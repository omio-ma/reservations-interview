import { apiClient } from "./client";
import { RoomListSchema, Room } from "../types/rooms";

export async function fetchRooms(): Promise<Room[]> {
  return apiClient.get("room").json().then(RoomListSchema.parseAsync);
}
