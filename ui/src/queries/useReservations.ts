import { useMutation } from "@tanstack/react-query";
import { bookRoom } from "../api/reservations";

interface useReservationsProps {
  onSuccess?: () => void;
  onError?: (error: string) => void;
}

export function useReservations({ onSuccess, onError }: useReservationsProps) {
  return useMutation({
    mutationFn: bookRoom,
    onSuccess: (result) => {
      if (result.success) {
        onSuccess?.();
      } else {
        onError?.(result.error || "Booking failed.");
      }
    },
    onError: () => {
      onError?.("An unexpected error occurred.");
    }
  });
}
