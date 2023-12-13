
namespace SolveChess.Logic.Helpers;

public static class UsernameHelper
{

    private static readonly string[] adjectives = {
            "Happy", "Brilliant", "Radiant", "Vibrant", "Glorious",
            "Joyful", "Exuberant", "Fantastic", "Spectacular", "Marvelous",
            "Cheerful", "Lively", "Delightful", "Enthusiastic", "Energetic",
            "Captivating", "Euphoric", "Optimistic", "Splendid", "Blissful",
            "Magical", "Inspirational", "Stellar", "Dazzling", "Elegant",
            "Ecstatic", "Buoyant", "Uplifting", "Resplendent", "Gratifying"
        };

    private static readonly string[] chessPieces = { "King", "Queen", "Bishop", "Knight", "Rook", "Pawn" };

    public static string GetRandomUsername()
    {
        var random = new Random();

        string randomAdjective = adjectives[random.Next(adjectives.Length)];
        string randomChessPiece = chessPieces[random.Next(chessPieces.Length)];
        int randomNumber = random.Next(1, 1000);

        return $"{randomAdjective}{randomChessPiece}{randomNumber}";
    }

}

