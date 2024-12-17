import { bookRoom } from "../api/reservations";
import { apiClient } from "../api/client";
import { fromDateStringToIso } from "../utils/datetime";

jest.mock("../api/client", () => ({
  apiClient: {
    post: jest.fn()
  }
}));

describe("bookRoom", () => {
  const mockBooking = {
    RoomNumber: "101",
    GuestEmail: "test@example.com",
    Start: fromDateStringToIso("2025-12-01T00:00:00.000Z"),
    End: fromDateStringToIso("2025-12-05T00:00:00.000Z")
  };

  it("should send the correct payload and return success", async () => {
    (apiClient.post as jest.Mock).mockResolvedValue({
      status: 200,
      ok: true
    });

    const result = await bookRoom(mockBooking);

    expect(apiClient.post).toHaveBeenCalledWith("reservation", {
      json: {
        RoomNumber: "101",
        GuestEmail: "test@example.com",
        Start: "2025-12-01T00:00:00.000Z",
        End: "2025-12-05T00:00:00.000Z"
      },
      headers: {
        "Content-Type": "application/json"
      }
    });

    expect(result).toEqual({ success: true });
  });

  it("should return an error if room is already booked (409)", async () => {
    (apiClient.post as jest.Mock).mockResolvedValue({
      status: 409,
      ok: false
    });

    const result = await bookRoom(mockBooking);

    expect(result).toEqual({
      success: false,
      error: "Room already booked for the selected dates."
    });
  });

  it("should return an unexpected error on failure", async () => {
    (apiClient.post as jest.Mock).mockRejectedValue(new Error("Network Error"));

    const result = await bookRoom(mockBooking);

    expect(result).toEqual({
      success: false,
      error: "Network Error"
    });
  });
});
