import { Box, Button, Paper, Typography } from "@mui/material";
import { useActivities } from "../../../lib/hooks/useActivitites";
import { useNavigate, useParams } from "react-router";
import {useForm} from 'react-hook-form';
import { useEffect } from "react";
import { ActivitySchema, activitySchema } from "../../../lib/schemas/activitySchema";
import { zodResolver } from "@hookform/resolvers/zod";
import TextInput from "../../../app/shared/components/TextInput";
import SelectInput from "../../../app/shared/components/SelectInput";
import { CategoryOptions } from "./CategoryOptions";
import DateTimeInput from "../../../app/shared/components/DateTimeInput";
import LocationInput from "../../../app/shared/components/LocationInput";

//TODO: If some value of LocationIQ is null, the client side should resolve the problem to send a correct request 
export default function ActivityForm() {
    
    const {control, reset, handleSubmit} = useForm<ActivitySchema>({
        mode: 'onTouched', // different modes to apply validation
        resolver: zodResolver(activitySchema),
    });

    const {id} = useParams();
    const {updateActivity, createActivity, activity, isLoadingActivity} = useActivities(id);
    const navigate = useNavigate();

    useEffect(() => {
        if(activity) reset({
            ...activity,
            location:{
                city: activity.city,
                venue: activity.venue,
                latitude: activity.latitude,
                longitude: activity.longitude
            }
        }); 
    }, [activity, reset]); //The purpouse is don't use the defatulValue prop, because reset will set the values if activity exists

    const onSubmit = async (data: ActivitySchema) => {
       const {location, ...rest } = data;
       const formatData = {...rest, ...location};
       try {
            if(activity){
                updateActivity.mutate({...activity, ...formatData}, {
                    onSuccess: () => navigate(`/activities/${activity.id}`)
                });
            }else{
                createActivity.mutate(formatData, {
                    onSuccess: (id) => navigate(`/activities/${id}`)
                });
            }
       } catch (error) {
            console.log(error)
       }
    }

    if(isLoadingActivity) return <Typography>Loading...</Typography>

  return (
    <Paper sx={{borderRadius: 2, padding: 3}}>
        <Typography variant="h5" gutterBottom color="primary">
            {activity ? 'Edit activity' : 'Create activity'}
        </Typography>
        <Box component="form" onSubmit={handleSubmit(onSubmit)} display="flex" flexDirection="column" gap= {3}>
            <TextInput label='Title' control={control} name='title'/>
            <TextInput label='Description' control={control} name='description'
            multiline rows={3}/>

            <Box display='grid' gridTemplateColumns='1fr 1fr' gap={3}>
                <SelectInput 
                    items={CategoryOptions} 
                    label='Category' 
                    control={control} 
                    name='category'
                />
                <DateTimeInput label='Date' control={control} name='date'/>
            </Box>

            <LocationInput control={control} label="Insert the Location" name="location"/>

            <Box display="flex" justifyContent="end" gap={3} >
                <Button color="inherit" onClick={()=>navigate(`/activities/${activity?.id}`)}>Cancel</Button>
                <Button 
                type="submit" 
                color="success" 
                variant="contained"
                disabled={updateActivity.isPending || createActivity.isPending}
                >Submit</Button>
            </Box>
        </Box>
    </Paper>
  )
}
