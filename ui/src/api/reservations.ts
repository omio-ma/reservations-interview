import { apiClient } from "./client";
import { NewReservation } from "../types/reservations";
import { toIsoStr } from "../utils/datetime";

export async function bookRoom(
  booking: NewReservation
): Promise<{ success: boolean; error?: string }> {
  try {
    const newReservation = {
      ...booking,
      Start: toIsoStr(booking.Start),
      End: toIsoStr(booking.End)
    };

    const response = await apiClient.post("reservation", {
      json: newReservation,
      headers: {
        "Content-Type": "application/json"
      }
    });

    if (response.status === 409) {
      return {
        success: false,
        error: "Room already booked for the selected dates."
      };
    }

    if (response.ok) {
      return { success: true };
    }

    return { success: false, error: "An unexpected error occurred." };
  } catch (error: unknown) {
    const errorMessage =
      error instanceof Error ? error.message : "An unexpected error occurred.";
    return {
      success: false,
      error: errorMessage
    };
  }
}
