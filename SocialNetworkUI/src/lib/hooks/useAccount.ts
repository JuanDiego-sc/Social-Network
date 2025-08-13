import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { LoginSchema } from "../schemas/loginSchema"
import agent from "../api/agent";
import { useLocation, useNavigate } from 'react-router';
import { RegisterSchema } from '../schemas/registerSchema';
import { toast } from 'react-toastify';

export const useAccount = () =>{
    const queryClient = useQueryClient();
    const navigate = useNavigate();
    const location = useLocation();

    const loginUser = useMutation({
        mutationFn : async (creds: LoginSchema) => {
            await agent.post('/login?useCookies=true', creds);
        },
        onSuccess: async () => {
            //Invalidate queries force to refetch data every time the users logs in
           await queryClient.invalidateQueries({
                queryKey: ['user']
           });
           navigate('/activities');
        }
    });

    const logoutUser = useMutation({
        mutationFn: async () => {
            await agent.post('/account/logout');
        },
        onSuccess: () =>{
            //remove the data 
            queryClient.removeQueries({queryKey: ['user']});
            queryClient.removeQueries({queryKey: ['activities']});
            navigate('/');
        }
    })

    const {data: currentUser, isLoading: loadingUserInfo} = useQuery({
        queryKey: ['user'],
        queryFn: async () => {
            const response = await agent.get<User>('/account/user-info');
            return response.data;
        },
        //just trying to get data if it does not exists 
        enabled: !queryClient.getQueryData(['user']) 
            && location.pathname !== '/register'
            && location.pathname !== '/login'
    });

    const registerUser = useMutation({
        mutationFn: async (creds: RegisterSchema) =>{
            await agent.post('/account/register', creds)
        },
        onSuccess: () => {
            toast.success('Success, you can now login');
            navigate('/login');
        }
    })

    return {
        loginUser,
        currentUser,
        logoutUser,
        loadingUserInfo,
        registerUser
    }
}