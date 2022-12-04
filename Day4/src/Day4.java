import java.io.*;
import java.util.regex.Pattern;

public class Day4 {
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
        var input = new File("inputs/day4.txt");
        var br = new BufferedReader(new FileReader(input));

        String line;
        String[] parts;
        String[] left;
        String[] right;

        int leftLower;
        int leftUpper;
        int rightLower;
        int rightUpper;

        int countContained = 0;
        int countOverlapped = 0;

        while ((line = br.readLine()) != null) {
            parts = line.split(",");
            left = parts[0].split("-");
            right = parts[1].split("-");

            leftLower = Integer.parseInt(left[0]);
            leftUpper = Integer.parseInt(left[1]);
            rightLower = Integer.parseInt(right[0]);
            rightUpper = Integer.parseInt(right[1]);

            // Check fully contained
            if (leftLower >= rightLower && leftUpper <= rightUpper || rightLower >= leftLower && rightUpper <= leftUpper) {
                ++countContained;
            }

            // Check any overlap
            if (leftLower <= rightUpper && rightLower <= leftUpper) {
                ++countOverlapped;
            }
        }

        System.out.println(countContained);
        System.out.println(countOverlapped);
    }
}