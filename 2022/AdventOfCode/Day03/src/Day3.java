import java.io.*;
import java.util.Arrays;

public class Day3 {
    public static void main(String[] args) throws IOException {
        //runParts();
        runCombined();
    }

    private static void runParts() throws IOException {
        long elapsedTimeSum = 0;
        for (int i = 0; i < 1000; i++) {
            long startTime = System.nanoTime();
            calculatePartOne();
            calculatePartTwo();
            long elapsedTime = System.nanoTime() - startTime;
            elapsedTimeSum += elapsedTime;
        }
        System.out.println("average execution time (ns): " + elapsedTimeSum/1000);
    }

    private static void runCombined() throws IOException {
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
        var input = new File("inputs/day3.txt");
        var br = new BufferedReader(new FileReader(input));

        boolean[] occursOne = new boolean[52];
        boolean[] occursTwo = new boolean[52];
        boolean[] occursThree = new boolean[52];

        String line;
        int sumP1 = 0;
        int sumP2 = 0;

        while ((line = br.readLine()) != null) {
            // Line n
            sumP1 += calculateLine(line, occursOne);

            // Line n+1
            line = br.readLine();
            sumP1 += calculateLine(line, occursTwo);

            // Line n+2
            line = br.readLine();
            sumP1 += calculateLine(line, occursThree);

            // Sum for part two
            for (int i = 0; i < 52; i++) {
                if (occursOne[i] && occursTwo[i] && occursThree[i]) {
                    sumP2 += i + 1;
                    break;
                }
            }

            Arrays.fill(occursOne, false);
            Arrays.fill(occursTwo, false);
            Arrays.fill(occursThree, false);
        }

        System.out.println(sumP1);
        System.out.println(sumP2);
    }

    private static int calculateLine(String line, boolean[] occursPart) {
        String left = line.substring(0, line.length() / 2);
        String right = line.substring(line.length() / 2);
        boolean[] occurs = new boolean[52];
        int sum = 0;

        for (var c : left.toCharArray()) {
            int index = c >= 'a' ? c - 'a' : c - 'A' + 26;
            occursPart[index] = true;

            occurs[index] = true;
        }

        for (var c : right.toCharArray()) {
            int index = c >= 'a' ? c - 'a' : c - 'A' + 26;
            occursPart[index] = true;

            if (occurs[index]) {
                sum += index + 1;

                // To prevent duplicates
                occurs[index] = false;
            }
        }

        return sum;
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