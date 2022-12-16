import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

public class Day13 {
    public static void main(String[] args) throws IOException {
        long elapsedTimeSum = 0;
        for (int i = 0; i < 1000; i++) {
            long startTime = System.nanoTime();

            parseInput();

            long elapsedTime = System.nanoTime() - startTime;
            elapsedTimeSum += elapsedTime;
        }
        System.out.println("average execution time (ns): " + elapsedTimeSum / 1000);
    }

    private static List<Pair> parseInput() throws IOException {
        var input = new java.io.File("inputs/examples/day13_ex.txt");
        var br = new BufferedReader(new FileReader(input));

        String line;
        var pairs = new ArrayList<Pair>();

        while ((line = br.readLine()) != null) {
            pairs.add(new Pair(line, br.readLine()));
            br.readLine();
        }

        return pairs;
    }


}

class Pair {
    String first;
    String second;

    public Pair(String first, String second) {
        this.first = first;
        this.second = second;
    }
}