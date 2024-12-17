import { apiClient } from "./client";
import { NewReservation } from "../types/reservations";
import { toIsoStr } from "../utils/datetime";

export async function bookRoom(booking: NewReservation): Promise<void> {
  const newReservation = {
    ...booking,
    Start: toIsoStr(booking.Start),
    End: toIsoStr(booking.End)
  };

  await apiClient.post("reservation", {
    json: newReservation,
    headers: {
      "Content-Type": "application/json"
    }
  });
}
