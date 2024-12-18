import { Box, Button, Dialog, Separator, TextField } from "@radix-ui/themes";
import { useState } from "react";
import { useShowInfoToast, useShowSuccessToast } from "../utils/toasts";
import { useNavigate } from "@tanstack/react-router";
import { useLogin } from "../queries/useLogin";

interface LoginModalProps {
  onClose: () => void;
}

export function LoginModal({ onClose }: LoginModalProps) {
  const [accessCode, setAccessCode] = useState("");

  const showSuccessToast = useShowSuccessToast("Login successful!");
  const showErrorToast = useShowInfoToast("Invalid access code. Try again.");
  const navigate = useNavigate();

  const { mutate: login, isPending } = useLogin({
    onSuccess: () => {
      showSuccessToast();
      onClose();
      navigate({ to: "/staff-page" });
    },
    onError: () => {
      showErrorToast();
    }
  });

  return (
    <Dialog.Content size="4">
      <Dialog.Title>Staff Login</Dialog.Title>
      <Dialog.Description>Enter your access code to log in</Dialog.Description>
      <Separator color="cyan" size="4" my="4" />
      <Box>
        <TextField.Root
          data-testid="access-code-input"
          placeholder="Enter access code..."
          onChange={(evt) => setAccessCode(evt.target.value)}
          value={accessCode}
          type="password"
          size="3"
          mb="4"
        />
      </Box>
      <Box style={{ display: "flex", justifyContent: "flex-end" }}>
        <Button
          data-testid="login-button"
          onClick={() => login(accessCode)}
          disabled={isPending}
          size="3"
          color="mint"
        >
          {isPending ? "Logging in..." : "Login"}
        </Button>
      </Box>
    </Dialog.Content>
  );
}
