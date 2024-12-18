import { renderHook, waitFor } from "@testing-library/react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { isStaffAuthenticated } from "../api/staff";
import { useIsStaffAuthenticated } from "./useIsStaffAuthenticated";

jest.mock("../api/staff", () => ({
  isStaffAuthenticated: jest.fn()
}));

describe("useIsStaffAuthenticated.ts", () => {
  const createQueryClient = () => new QueryClient();

  const wrapper = ({ children }: { children: React.ReactNode }) => (
    <QueryClientProvider client={createQueryClient()}>
      {children}
    </QueryClientProvider>
  );

  it("should return true when staff is authenticated", async () => {
    (isStaffAuthenticated as jest.Mock).mockResolvedValue(true);

    const { result } = renderHook(() => useIsStaffAuthenticated(), { wrapper });

    await waitFor(() => expect(result.current.isSuccess).toBe(true));

    expect(result.current.data).toBe(true);
  });

  it("should return false when staff is not authenticated", async () => {
    (isStaffAuthenticated as jest.Mock).mockResolvedValue(false);

    const { result } = renderHook(() => useIsStaffAuthenticated(), { wrapper });

    await waitFor(() => expect(result.current.isSuccess).toBe(true));

    expect(result.current.data).toBe(false);
  });
});
