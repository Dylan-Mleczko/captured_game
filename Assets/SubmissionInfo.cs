public static class SubmissionInfo
{

  public static readonly string RepoURL = "https://github.com/COMP30019/project-2-shorts";

  public static readonly string TeamName = "かわいい　ハワイ　SHORTS";

  public static readonly TeamMember[] Team = new[]
  {
        new TeamMember("Thomas Robertson", "tjrobertson@student.unimelb.edu.au"),
        new TeamMember("Kaiyi Sun", "kaiyis1@student.unimelb.edu.au"),
        new TeamMember("Dylan Cookson-Mleczko", "dcooksonmlec@student.unimelb.edu.au"),
    };

  public static readonly string GameName = "Captured";

  // Write a brief blurb of your game, no more than 200 words. Again, ensure
  // this is final by the video milestone deadline.
  public static readonly string GameBlurb =
    @"CLANG! The cell door slams shut behind you. You thought they'd never find you,
    but in the end they always seem to win. For centuries the ruling class of royalty,
    knights and bishops have treated your people, the pawns, like... well... pawns,
    but you never thought it would turn to this. You thought you were safe in the
    rebel camp as you bided your time for an attack, but before you knew it they were
    marching in and massacring your family and comrades. As the leader of the resistance
    they're keeping you alive to rot in jail before your mock trial where you will
    be made a thorough example of. But do things really have to end this way? Sure,
    you may be alone and outnumbered, but there's still time for the game to turn.
    All you have to do is break out, kill the king, and free your people, all while
    avoiding capture and solving puzzles along the way. Are you up to the challenge?
    Explore, hide, and exact your revenge in this new chess-themed first-person horror
    game in the style of Amnesia, developed and presented by かわいい　ハワイ SHORTS!";

  // By the gameplay video milestone deadline this should be a direct link
  // to a YouTube video upload containing your video. Ensure "Made for kids"
  // is turned off in the video settings. 
  public static readonly string GameplayVideo = "https://youtu.be/U2HQTEUbJNs";

  public readonly struct TeamMember
  {
    public TeamMember(string name, string email)
    {
      Name = name;
      Email = email;
    }

    public string Name { get; }
    public string Email { get; }
  }
}