import { useEffect, useState } from "react";
import { Activity } from "./lib/types";
import { List, ListItem, ListItemText, Typography } from "@mui/material";
import axios from "axios";


function App() {
  const [activities, setActivities] = useState<Activity[]>([]);

  useEffect(() => {
    axios.get<Activity[]>('https://localhost:5001/api/activities')
      .then((response) => setActivities(response.data))
    }, []);


  return (
    <>
      <Typography variant='h3'> Social Network </Typography>
      <List>
        {activities.map((activity) => (
          <ListItem key={activity.id}>
            <ListItemText>{activity.title}</ListItemText>
            <ListItemText>{activity.description}</ListItemText>
            <ListItemText>{activity.date}</ListItemText>
            <ListItemText>{activity.city}</ListItemText>
            <ListItemText>{activity.venue}</ListItemText>
            <ListItemText>{activity.isCancelled ? 'was Cancelled' : 'avalible'} </ListItemText>
          </ListItem>
        ))}
      </List>
    </>
  )
}

export default App
