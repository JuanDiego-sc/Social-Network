import { keepPreviousData, useInfiniteQuery, useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import agent from "../api/agent";
import { useLocation } from "react-router";
import { FieldValues } from "react-hook-form";
import { useAccount } from "./useAccount";
import { useStore } from "./useStore";

export const useActivities = (id?: string) => {
    const {activityStore: {filter, startDate}} = useStore();
    const queryClient = useQueryClient();
    const {currentUser} = useAccount();
    const location = useLocation();

    const {data: activitiesGroup, isLoading, isFetchingNextPage, fetchNextPage, hasNextPage} 
        = useInfiniteQuery<PagedList<Activity, string>>({
        queryKey: ['activities', filter, startDate],
        queryFn: async ({pageParam = null}) => {
          const response = await agent.get<PagedList<Activity, string>>('/activities', {
            params: {
                cursor: pageParam,
                pageSize: 3,
                filter,
                startDate
            }
          });
          return response.data;
        },
        initialPageParam: null,
        getNextPageParam: (lastPage) => lastPage.nextCursor,
        staleTime: 1000 * 60 * 5,
        placeholderData: keepPreviousData,
        //Just work if there's no an id and the path is /activities and the user is loged
        enabled: !id && location.pathname ==='/activities' && !!currentUser,
        select: data => ({
            ...data,
            pages: data.pages.map((page) => ({
                ...page,
                items: page.items.map(activity => {
                    const host = activity?.attendees.find(x => x.id === activity.hostId);
                    return {
                    ...activity,
                    isHost: currentUser?.id === activity.hostId,
                    isGoing: activity.attendees.some(x => x.id === currentUser?.id),
                    hostImageUrl: host?.imageUrl
                    }
                })
            }))
        })
      });

      const {data: activity, isLoading: isLoadingActivity} = useQuery({
        queryKey: ['activities', id],
        queryFn: async () => {
          const response = await agent.get<Activity>(`/activities/${id}`);
          return response.data;
        },
        enabled: !!id && !!currentUser, //if id exists and an user is loged, get an activity going to work
        select: data => {
            const host = data?.attendees.find(x => x.id === data.hostId);
            return {
                ...data,
                isHost: currentUser?.id === data.hostId,
                isGoing: data.attendees.some(x => x.id === currentUser?.id),
                hostImageUrl: host?.imageUrl
            }
        }
      });

    const updateActivity = useMutation({
        mutationFn: async (activty: Activity) =>{
            await agent.put('/activities', activty);
        },
        onSuccess: async () => {
            await queryClient.invalidateQueries({
                queryKey: ['activities']
            })
        }
    });

    const createActivity = useMutation({
        mutationFn: async (activty: FieldValues) =>{
            const response = await agent.post('/activities', activty);
            return response.data;
        },
        onSuccess: async () => {
            await queryClient.invalidateQueries({
                queryKey: ['activities']
            });
        }
    });

    const deleteActivity = useMutation({
        mutationFn: async (id : string) => {
            await agent.delete(`/activities/${id}`);
        },
        onSuccess: async () => {
            await queryClient.invalidateQueries({
                queryKey: ['activities']
            });
        }
    });

    const updateAttendence = useMutation({
        mutationFn: async (id : string) => {
            await agent.post(`/activities/${id}/attend`)
        },
        onMutate: async (activityId: string)  => {
            await queryClient.cancelQueries({queryKey: ['activities', activityId]});

            const prevActivity = queryClient.getQueryData<Activity>(['activities', activityId]);

            queryClient.setQueryData<Activity>(['activities', activityId], oldActivity => {
                if(!oldActivity || !currentUser){
                    return oldActivity
                }

                const isHost = oldActivity.hostId === currentUser.id;
                const isAttending = oldActivity.attendees.some(x => x.id === currentUser.id);

                return {
                    ...oldActivity,
                    isCancelled : isHost 
                    ? !oldActivity.isCancelled 
                    : oldActivity.isCancelled,

                    attendees: isAttending 
                    ? isHost
                        ? oldActivity.attendees
                        : oldActivity.attendees.filter(x => x.id !== currentUser.id)
                    : [...oldActivity.attendees, {
                        id : currentUser.id,
                        displayName: currentUser.displayName,
                        imageUrl : currentUser.imageUrl
                    }]
                }
            });
            return {prevActivity};
        },
        onError: (error, activityId, context) => {
            console.log(error);
            if(context?.prevActivity){
                queryClient.setQueryData(['activities', activityId], context.prevActivity)
            }
        }
    })
    

      return{
        activitiesGroup,
        isFetchingNextPage,
        fetchNextPage,
        hasNextPage,
        isLoading,
        updateActivity,
        createActivity,
        deleteActivity,
        activity,
        isLoadingActivity,
        updateAttendence
      }
}