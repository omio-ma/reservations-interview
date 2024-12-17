import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import { ReservationPage } from "./ReservationPage";
import { useReservations } from "../queries/useReservations";
import { useGetRooms } from "../queries/useGetRooms";

jest.mock("../queries/useGetRooms", () => ({
  useGetRooms: jest.fn()
}));

jest.mock("../queries/useReservations", () => ({
  useReservations: jest.fn()
}));

jest.mock("./BookingDetailsModal", () => ({
  BookingDetailsModal: ({ onSubmit }: { onSubmit: (booking: any) => void }) => {
    onSubmit({
      RoomNumber: "101",
      GuestEmail: "test@example.com",
      Start: "2025-12-01T00:00:00.000Z",
      End: "2025-12-05T00:00:00.000Z"
    });
    return null;
  }
}));

describe("ReservationPage", () => {
  it("submits the booking", async () => {
    const mockMutate = jest.fn();

    (useGetRooms as jest.Mock).mockReturnValue({
      isLoading: false,
      data: [{ number: "101", state: 1 }]
    });

    (useReservations as jest.Mock).mockReturnValue({
      mutate: mockMutate
    });

    render(<ReservationPage />);

    await waitFor(() => {
      expect(mockMutate).toHaveBeenCalledWith({
        RoomNumber: "101",
        GuestEmail: "test@example.com",
        Start: "2025-12-01T00:00:00.000Z",
        End: "2025-12-05T00:00:00.000Z"
      });
    });
  });
});
