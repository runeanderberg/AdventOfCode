import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.util.Arrays;
import java.util.HashSet;
import java.util.Set;

public class Day3 {
    public static void main(String[] args) throws IOException {
        long startTime = System.nanoTime();
        calculatePartOne();
        calculatePartTwo();
        long elapsedTime = System.nanoTime() - startTime;
        System.out.println("execution time (ms): " + elapsedTime/1000000);
    }

    private static void calculatePartOne() throws IOException {
        var input = new File("inputs/day3.txt");

        var br = new BufferedReader(new FileReader(input));

        boolean[] occurs = new boolean[52];

        String line;
        String left;
        String right;
        int sum = 0;

        while ((line = br.readLine()) != null) {
            left = line.substring(0, line.length() / 2);
            right = line.substring(line.length() / 2);

            for (var c : left.toCharArray()) {
                int index = c >= 'a' ? c - 'a' : c - 'A' + 26;
                occurs[index] = true;
            }

            for (var c : right.toCharArray()) {
                int index = c >= 'a' ? c - 'a' : c - 'A' + 26;

                if (occurs[index]) {
                    sum += index + 1;

                    // To prevent duplicates
                    occurs[index] = false;
                }
            }

            Arrays.fill(occurs, false);
        }

        System.out.println(sum);
    }

    private static void calculatePartTwo() throws IOException {
        var input = new File("inputs/day3.txt");

        var br = new BufferedReader(new FileReader(input));

        boolean[] occursOne = new boolean[52];
        boolean[] occursTwo = new boolean[52];
        boolean[] occursThree = new boolean[52];

        String line;
        int sum = 0;

        while ((line = br.readLine()) != null) {
            for (var c : line.toCharArray()) {
                int index = c >= 'a' ? c - 'a' : c - 'A' + 26;
                occursOne[index] = true;
            }

            line = br.readLine();
            for (var c : line.toCharArray()) {
                int index = c >= 'a' ? c - 'a' : c - 'A' + 26;
                occursTwo[index] = true;
            }

            line = br.readLine();
            for (var c : line.toCharArray()) {
                int index = c >= 'a' ? c - 'a' : c - 'A' + 26;
                occursThree[index] = true;
            }

            for (int i = 0; i < 52; i++) {
                if (occursOne[i] && occursTwo[i] && occursThree[i]) {
                    sum += i + 1;
                    break;
                }
            }

            Arrays.fill(occursOne, false);
            Arrays.fill(occursTwo, false);
            Arrays.fill(occursThree, false);
        }

        System.out.println(sum);
    }
}