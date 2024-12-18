import { renderHook } from "@testing-library/react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { useLogin } from "./useLogin";
import { login } from "../api/staff";

jest.mock("../api/staff", () => ({
  login: jest.fn()
}));

describe("useLogin.ts", () => {
  const queryClient = new QueryClient();

  const wrapper = ({ children }: { children: React.ReactNode }) => (
    <QueryClientProvider client={queryClient}>{children}</QueryClientProvider>
  );

  it("should call onSuccess when login succeeds", async () => {
    (login as jest.Mock).mockResolvedValue(true);
    const onSuccess = jest.fn();
    const onError = jest.fn();

    const { result } = renderHook(() => useLogin({ onSuccess, onError }), {
      wrapper
    });

    await result.current.mutateAsync("valid-code");

    expect(login).toHaveBeenCalledWith("valid-code");
    expect(onSuccess).toHaveBeenCalled();
    expect(onError).not.toHaveBeenCalled();
  });

  it("should call onError when login fails", async () => {
    (login as jest.Mock).mockResolvedValue(false);
    const onSuccess = jest.fn();
    const onError = jest.fn();

    const { result } = renderHook(() => useLogin({ onSuccess, onError }), {
      wrapper
    });

    await result.current.mutateAsync("invalid-code");

    expect(login).toHaveBeenCalledWith("invalid-code");
    expect(onSuccess).not.toHaveBeenCalled();
    expect(onError).toHaveBeenCalled();
  });
});
