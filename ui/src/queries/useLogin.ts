import { useMutation } from "@tanstack/react-query";
import { login } from "../api/staff";

interface UseLoginProps {
  onSuccess?: () => void;
  onError?: () => void;
}

export function useLogin({ onSuccess, onError }: UseLoginProps) {
  return useMutation({
    mutationFn: (accessCode: string) => login(accessCode),
    onSuccess: (authenticated) => {
      if (authenticated) {
        onSuccess?.();
      } else {
        onError?.();
      }
    },
    onError: () => onError?.()
  });
}
