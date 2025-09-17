namespace TicketHive.Application.DTOs.Users;

public record UserDTO(Guid Id, string Email, string FullName, string PhoneNumber , string Role);