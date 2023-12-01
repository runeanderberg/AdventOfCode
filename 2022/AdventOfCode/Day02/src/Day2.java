import java.io.*;

public class Day2 {
    public static void main(String[] args) throws IOException {
        long elapsedTimeSum = 0;
        for (int i = 0; i < 1000; i++) {
            long startTime = System.nanoTime();
            calculate();
            long elapsedTime = System.nanoTime() - startTime;
            elapsedTimeSum += elapsedTime;
        }
        System.out.println("average execution time (ns): " + elapsedTimeSum/1000);
    }

    private static void calculate() throws IOException {
        var input = new File("inputs/day2.txt");
        var br = new BufferedReader(new FileReader(input));

        int opponent;
        int self;
        int scoreP1 = 0;
        int scoreP2 = 0;

        int[] scoreTableP1 = {4, 1, 7,
                              8, 5, 2,
                              3, 9, 6};

        int[] scoreTableP2 = {3, 1, 2,
                              4, 5, 6,
                              8, 9, 7};

        while ((opponent = br.read()) != -1) {
            br.read();
            self = br.read();

            // "Normalize" moves
            opponent -= 'A';
            self -= 'X';

            scoreP1 += scoreTableP1[self * 3 + opponent];
            scoreP2 += scoreTableP2[self * 3 + opponent];

            br.read();
            br.read();
        }

        System.out.println(scoreP1);
        System.out.println(scoreP2);
    }
}