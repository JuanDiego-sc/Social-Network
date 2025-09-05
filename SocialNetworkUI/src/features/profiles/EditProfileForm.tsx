import { useParams } from "react-router";
import { useProfile } from "../../lib/hooks/useProfile";
import {
  editProfileSchema,
  EditProfileSchema,
} from "../../lib/schemas/EditProfileSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { useEffect } from "react";
import { Box, Button } from "@mui/material";
import TextInput from "../../app/shared/components/TextInput";

type Props = {
  setEditMode: (editMode: boolean) => void;
};

export default function EditProfileForm({ setEditMode }: Props) {
  const { id } = useParams();
  const { profile, editProfile } = useProfile(id);
  const {
    control,
    handleSubmit,
    reset,
    formState: { isValid },
  } = useForm<EditProfileSchema>({
    mode: "onTouched",
    resolver: zodResolver(editProfileSchema),
  });

  const onSubmit = async (data: EditProfileSchema) => {
    await editProfile.mutateAsync(data, {
      onSuccess: () => {
        setEditMode(false);
      },
    });
  };

  useEffect(() => {
    reset({
      displayName: profile?.displayName,
      bio: profile?.bio || "",
    });
  }, [profile, reset]);

  return (
    <Box
      component="form"
      onSubmit={handleSubmit(onSubmit)}
      display="flex"
      flexDirection="column"
      alignContent="center"
      gap={3}
      mt={3}
    >
      <TextInput label="Display Name" name="displayName" control={control} />
      <TextInput
        label="Add your bio"
        name="bio"
        control={control}
        multiline
        rows={4}
      />
      <Button
        type="submit"
        variant="contained"
        disabled={!isValid || editProfile.isPending}
      >
        Update profile
      </Button>
    </Box>
  );
}
