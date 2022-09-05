/**
 * Name: Jordan Huynh
 * ID: A16990643
 * Email: johuynh@ucsd.edu
 * Sources used: None
 * 
 * Main file of the RPS game. Contains the main method as well as the
 * constructor and instantiates the determineWinner method. 
 */

import java.util.Scanner;

/**
 * Implements the game and determines the winner of every round.
 */
public class RPS extends RPSAbstract {
    
    /**
     * RPS game constructor. Initializes possibleMoves, playerMoves, and
     * cpuMoves.
     */
    public RPS(String[] moves) {
        this.possibleMoves = moves;
        this.playerMoves = new String[MAX_GAMES];
        this.cpuMoves = new String[MAX_GAMES];
    }

    /**
     * Takes the user move, the CPU move, and determines who wins.
     * @param playerMove - move of the player
     * @param cpuMove - move of the CPU
     * @return -1 for invalid move, 0 for tie, 1 for player win, 2 for cpu win
     */
    public int determineWinner(String playerMove, String cpuMove)
    {
        if (!isValidMove(playerMove)) {
            return INVALID_INPUT_OUTCOME;
        }
        if (!isValidMove(cpuMove)) {
            return INVALID_INPUT_OUTCOME;
        }

        int playerMoveIndex = 0;
        int cpuMoveIndex = 0;
        for (int i = 0; i < possibleMoves.length; i++) {

            if (possibleMoves[i].equals(playerMove)) {
                playerMoveIndex = i;
                }
            
            if (possibleMoves[i].equals(cpuMove)) {
                cpuMoveIndex = i;
            }
        }
  
        //This is for when one of the moves is at the end of the moves array.
        //It will wrap back to the first element to determine a winner. 
        if (playerMoveIndex == possibleMoves.length - 1) {
            if (cpuMoveIndex == 0) {
                return PLAYER_WIN_OUTCOME;
            }
        }

        else if (cpuMoveIndex == possibleMoves.length - 1) {
            if (playerMoveIndex == 0) {
                return CPU_WIN_OUTCOME;
            }
        }
        
        if (playerMoveIndex == cpuMoveIndex - 1) {
            return PLAYER_WIN_OUTCOME;
        }

        else if (cpuMoveIndex == playerMoveIndex - 1) {
            return CPU_WIN_OUTCOME;
        }

        return TIE_OUTCOME;
    }

    /**
     * Main method that reads user input, generates cpu move, and plays game
     * 
     * @param args - arguments passed in from command line in String form
     */
    public static void main(String[] args) {
        // If command line args are provided use those as the possible moves
        String[] moves = new String[args.length];
        if (args.length >= MIN_POSSIBLE_MOVES) {
            for (int i = 0; i < args.length; i++) {
                moves[i] = args[i];
            }
        } else {
            moves = RPS.DEFAULT_MOVES;
        }

        // Create new game and scanner
        RPS game = new RPS(moves);
        Scanner in = new Scanner(System.in);

        System.out.println(PROMPT_MOVE);
        String command = in.nextLine();

        // While user does not input "q", play game 
        while (!command.equals("q")) {
            String cpu = game.genCPUMove();
            game.play(command, cpu);
            System.out.println(PROMPT_MOVE);
            command = in.nextLine();
        }

        game.end();
        // See the writeup for an example of the game play.
        // Hint: call the methods we/you have already written 
        // to do most of the work!


        in.close();
    }
}
