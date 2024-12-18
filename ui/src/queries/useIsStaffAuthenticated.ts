import { useQuery } from "@tanstack/react-query";
import { isStaffAuthenticated } from "../api/staff";

export function useIsStaffAuthenticated() {
  return useQuery({
    queryKey: ["staffAuth"],
    queryFn: isStaffAuthenticated,
    retry: false
  });
}
