namespace concert.API.IntegrationEvents.Events;

public record NewConcertCreatedIntegrationEvent(int ConcertId, string ArtistName, string VenueName) : IntegrationEvent;