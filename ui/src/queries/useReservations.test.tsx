import React from "react";
import { renderHook, act } from "@testing-library/react";
import { useReservations } from "./useReservations";
import { bookRoom } from "../api/reservations";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { fromDateStringToIso } from "../utils/datetime";

jest.mock("../api/reservations", () => ({
  bookRoom: jest.fn()
}));

const queryClient = new QueryClient();

function wrapper({ children }: { children?: React.ReactNode }) {
  return (
    <QueryClientProvider client={queryClient}>{children}</QueryClientProvider>
  );
}

describe("useReservations", () => {
  it("should call onSuccess when booking is successful", async () => {
    (bookRoom as jest.Mock).mockResolvedValue({ success: true });
    const onSuccess = jest.fn();

    const { result } = renderHook(() => useReservations({ onSuccess }), {
      wrapper
    });

    await act(async () => {
      await result.current.mutateAsync({
        RoomNumber: "101",
        GuestEmail: "test@example.com",
        Start: fromDateStringToIso("2025-12-01T00:00:00.000Z"),
        End: fromDateStringToIso("2025-12-05T00:00:00.000Z")
      });
    });

    expect(onSuccess).toHaveBeenCalled();
  });

  it("should call onError with the provided error message when booking fails", async () => {
    (bookRoom as jest.Mock).mockResolvedValue({
      success: false,
      error: "No rooms available"
    });
    const onError = jest.fn();

    const { result } = renderHook(() => useReservations({ onError }), {
      wrapper
    });

    await act(async () => {
      await result.current.mutateAsync({
        RoomNumber: "101",
        GuestEmail: "test@example.com",
        Start: fromDateStringToIso("2025-12-01T00:00:00.000Z"),
        End: fromDateStringToIso("2025-12-05T00:00:00.000Z")
      });
    });

    expect(onError).toHaveBeenCalledWith("No rooms available");
  });
});
