import java.io.*;
import java.util.Arrays;

public class Day6 {
    public static void main(String[] args) throws IOException {
        long elapsedTimeSum = 0;
        for (int i = 0; i < 1000; i++) {
            long startTime = System.nanoTime();
            solvePartOne();
            solvePartTwo();
            long elapsedTime = System.nanoTime() - startTime;
            elapsedTimeSum += elapsedTime;
        }
        System.out.println("average execution time (ns): " + elapsedTimeSum/1000);
    }

    private static void solvePartOne() throws IOException {
        var input = new File("inputs/day6.txt");
        var br = new BufferedReader(new FileReader(input));

        int[] previous = new int[4];

        // Pre-read first 3 chars
        for (int i = 0; i < 3; i++) {
            previous[i] = br.read();
        }

        int read;
        int readCount = 3;
        int placeIndex = 3;
        boolean hasDuplicates;

        while ((read = br.read()) != -1) {
            ++readCount;

            previous[placeIndex] = read;

            hasDuplicates = false;
            for (int i = 0; i < previous.length; i++) {
                for (int j = i + 1; j < previous.length; j++) {
                    if (previous[i] == previous[j]) {
                        hasDuplicates = true;
                        break;
                    }
                }
            }

            if (!hasDuplicates) {
                break;
            }

            ++placeIndex;
            if (placeIndex == 4) {
                placeIndex = 0;
            }
        }

        System.out.println(readCount);
    }

    private static void solvePartTwo() throws IOException {
        var input = new File("inputs/day6.txt");
        var br = new BufferedReader(new FileReader(input));

        int[] previous = new int[14];

        // Pre-read first 3 chars
        for (int i = 0; i < 13; i++) {
            previous[i] = br.read();
        }

        int read;
        int readCount = 13;
        int placeIndex = 13;
        boolean hasDuplicates;

        int[] occurrences = new int[26];

        while ((read = br.read()) != -1) {
            ++readCount;

            previous[placeIndex] = read;

            Arrays.fill(occurrences, 0);
            for (int c : previous) {
                ++occurrences[c - 'a'];
            }

            hasDuplicates = false;
            for (int occurrence : occurrences) {
                if (occurrence > 1) {
                    hasDuplicates = true;
                    break;
                }
            }

            if (!hasDuplicates) {
                break;
            }

            ++placeIndex;
            if (placeIndex == 14) {
                placeIndex = 0;
            }
        }

        System.out.println(readCount);
    }
}