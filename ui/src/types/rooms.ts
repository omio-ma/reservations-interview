import { z } from "zod";

export const RoomSchema = z.object({
  number: z.string(),
  state: z.number()
});

export type Room = z.infer<typeof RoomSchema>;
export const RoomListSchema = RoomSchema.array();
