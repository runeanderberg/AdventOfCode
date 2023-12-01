import java.io.*;

public class Day4 {
    public static void main(String[] args) throws IOException {
        long elapsedTimeSum = 0;
        for (int i = 0; i < 10000; i++) {
            long startTime = System.nanoTime();
            calculateV2();
            long elapsedTime = System.nanoTime() - startTime;
            elapsedTimeSum += elapsedTime;
        }
        System.out.println("average execution time (ns): " + elapsedTimeSum/10000);
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

    private static void calculateV2() throws IOException {
        var input = new File("inputs/day4.txt");
        var br = new BufferedReader(new FileReader(input));

        int read;

        int leftLower;
        int leftUpper;
        int rightLower;
        int rightUpper;

        int countContained = 0;
        int countOverlapped = 0;

        while ((read = br.read()) != -1) {
            leftLower = read - '0';

            read = br.read();
            if (read != '-') {
                leftLower *= 10;
                leftLower += read - '0';

                // discards '-'
                br.read();
            }

            leftUpper = br.read() - '0';

            read = br.read();
            if (read != ',') {
                leftUpper *= 10;
                leftUpper += read - '0';

                // discards ','
                br.read();
            }

            rightLower = br.read() - '0';

            read = br.read();
            if (read != '-') {
                rightLower *= 10;
                rightLower += read - '0';

                // discards '-'
                br.read();
            }

            rightUpper = br.read() - '0';

            read = br.read();
            if (read != '\r') {
                rightUpper *= 10;
                rightUpper += read - '0';

                // discards '\r' & '\n'
                br.read();
                br.read();
            } else {
                // discards '\n'
                br.read();
            }

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