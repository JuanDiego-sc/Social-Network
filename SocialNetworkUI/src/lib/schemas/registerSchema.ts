import { z } from 'zod';
import { requiredString } from '../util/util';

export const registerSchema = z.object({
    email: z.string().email(),
    password: requiredString('password'),
    displayName: requiredString("Display Name"),
});

export type RegisterSchema = z.infer<typeof registerSchema>;