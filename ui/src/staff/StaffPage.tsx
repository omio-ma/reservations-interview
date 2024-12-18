import { Box, Heading } from "@radix-ui/themes";

export function StaffPage() {
  return (
    <Box p="6">
      <Heading size="6" as="h1" align="center" mb="6">
        Staff Home page
      </Heading>
      <p>
        For showing reservations and other features relating to managing
        reservations
      </p>
    </Box>
  );
}
