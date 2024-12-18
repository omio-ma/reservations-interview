import { render, screen, fireEvent } from "@testing-library/react";
import { Dialog } from "@radix-ui/themes";
import { LoginModal } from "./LoginModal";
import { useLogin } from "../queries/useLogin";

jest.mock("../queries/useLogin", () => ({
  useLogin: jest.fn()
}));

jest.mock("@tanstack/react-router", () => ({
  useNavigate: () => jest.fn()
}));

describe("LoginModal.tsx", () => {
  it("should call login with the entered access code", async () => {
    const mockMutate = jest.fn();
    const mockOnClose = jest.fn();

    (useLogin as jest.Mock).mockReturnValue({
      mutate: mockMutate,
      isPending: false
    });

    render(
      <Dialog.Root open>
        <LoginModal onClose={mockOnClose} />
      </Dialog.Root>
    );

    const input = screen.getByTestId("access-code-input");
    const button = screen.getByTestId("login-button");

    fireEvent.change(input, { target: { value: "test-code" } });
    fireEvent.click(button);

    expect(mockMutate).toHaveBeenCalledWith("test-code");
  });

  it("should disable the button when login is pending", async () => {
    (useLogin as jest.Mock).mockReturnValue({
      mutate: jest.fn(),
      isPending: true
    });

    render(
      <Dialog.Root open>
        <LoginModal onClose={jest.fn()} />
      </Dialog.Root>
    );

    const button = screen.getByTestId("login-button");
    expect(button).toBeDisabled();
  });

  it("should call onClose and navigate on successful login", async () => {
    const mockMutate = jest.fn((_, options?: any) => {
      options?.onSuccess();
    });

    (useLogin as jest.Mock).mockReturnValue({
      mutate: mockMutate,
      isPending: false
    });

    render(
      <Dialog.Root open>
        <LoginModal onClose={jest.fn()} />
      </Dialog.Root>
    );

    const input = screen.getByTestId("access-code-input");
    const button = screen.getByTestId("login-button");

    fireEvent.change(input, { target: { value: "test-code" } });
    fireEvent.click(button);

    expect(mockMutate).toHaveBeenCalledWith("test-code");
  });
});
