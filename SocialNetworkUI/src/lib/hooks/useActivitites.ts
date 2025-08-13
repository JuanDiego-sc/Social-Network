import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import agent from "../api/agent";
import { useLocation } from "react-router";
import { FieldValues } from "react-hook-form";
import { useAccount } from "./useAccount";

export const useActivities = (id?: string) => {

    const queryClient = useQueryClient();
    const {currentUser} = useAccount();
    const location = useLocation();

    const {data: activities, isLoading} = useQuery({
        queryKey: ['activities'],
        queryFn: async () => {
          const response = await agent.get<Activity[]>('/activities');
          return response.data;
        },
        //Just work if there's no an id and the path is /activities and the user is loged
        enabled: !id && location.pathname ==='/activities' && !!currentUser 

      });

      const {data: activity, isLoading: isLoadingActivity} = useQuery({
        queryKey: ['activities', id],
        queryFn: async () => {
          const response = await agent.get<Activity>(`/activities/${id}`);
          return response.data;
        },
        enabled: !!id && !!currentUser //if id exists and an user is loged, get an activity going to work
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
    

      return{
        activities,
        isLoading,
        updateActivity,
        createActivity,
        deleteActivity,
        activity,
        isLoadingActivity
      }
}