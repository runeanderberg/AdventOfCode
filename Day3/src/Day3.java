import java.io.*;
import java.util.Arrays;

public class Day3 {
    public static void main(String[] args) throws IOException {
        long startTime = System.nanoTime();
        //calculatePartOne();
        //calculatePartTwo();
        calculate();
        long elapsedTime = System.nanoTime() - startTime;
        System.out.println("execution time (ms): " + elapsedTime/1000000);
    }

    private static void calculate() throws IOException {
        var input = new File("inputs/day3.txt");
        var br = new BufferedReader(new FileReader(input));

        boolean[] occurs = new boolean[52];
        boolean[] occursOne = new boolean[52];
        boolean[] occursTwo = new boolean[52];
        boolean[] occursThree = new boolean[52];

        String line;
        String left;
        String right;
        int sumP1 = 0;
        int sumP2 = 0;

        while ((line = br.readLine()) != null) {
            // Line n
            left = line.substring(0, line.length() / 2);
            right = line.substring(line.length() / 2);

            for (var c : left.toCharArray()) {
                int index = c >= 'a' ? c - 'a' : c - 'A' + 26;
                occursOne[index] = true;

                occurs[index] = true;
            }

            for (var c : right.toCharArray()) {
                int index = c >= 'a' ? c - 'a' : c - 'A' + 26;
                occursOne[index] = true;

                if (occurs[index]) {
                    sumP1 += index + 1;

                    // To prevent duplicates
                    occurs[index] = false;
                }
            }

            Arrays.fill(occurs, false);

            // Line n+1
            line = br.readLine();
            left = line.substring(0, line.length() / 2);
            right = line.substring(line.length() / 2);

            for (var c : left.toCharArray()) {
                int index = c >= 'a' ? c - 'a' : c - 'A' + 26;
                occursTwo[index] = true;

                occurs[index] = true;
            }

            for (var c : right.toCharArray()) {
                int index = c >= 'a' ? c - 'a' : c - 'A' + 26;
                occursTwo[index] = true;

                if (occurs[index]) {
                    sumP1 += index + 1;

                    // To prevent duplicates
                    occurs[index] = false;
                }
            }

            Arrays.fill(occurs, false);

            // Line n+2
            line = br.readLine();
            left = line.substring(0, line.length() / 2);
            right = line.substring(line.length() / 2);

            for (var c : left.toCharArray()) {
                int index = c >= 'a' ? c - 'a' : c - 'A' + 26;
                occursThree[index] = true;

                occurs[index] = true;
            }

            for (var c : right.toCharArray()) {
                int index = c >= 'a' ? c - 'a' : c - 'A' + 26;
                occursThree[index] = true;

                if (occurs[index]) {
                    sumP1 += index + 1;

                    // To prevent duplicates
                    occurs[index] = false;
                }
            }

            Arrays.fill(occurs, false);

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