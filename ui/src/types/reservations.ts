import { z } from "zod";
import { ISO8601String } from "../utils/datetime";

export const ReservationSchema = z.object({
  Id: z.string(),
  RoomNumber: z.string(),
  GuestEmail: z.string().email(),
  Start: z.string(),
  End: z.string()
});

export type Reservation = z.infer<typeof ReservationSchema>;

export interface NewReservation {
  RoomNumber: string;
  GuestEmail: string;
  Start: ISO8601String;
  End: ISO8601String;
}
