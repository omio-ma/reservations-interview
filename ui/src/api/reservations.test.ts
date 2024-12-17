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

  it("should send the correct payload", async () => {
    await bookRoom(mockBooking);

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
  });
});
