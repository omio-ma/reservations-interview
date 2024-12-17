import { useQuery } from "@tanstack/react-query";
import { fetchRooms } from "../api/rooms";
import { Room } from "../types/rooms";

export function useGetRooms() {
  return useQuery<Room[]>({
    queryKey: ["rooms"],
    queryFn: fetchRooms
  });
}
