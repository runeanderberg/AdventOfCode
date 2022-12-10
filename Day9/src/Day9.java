import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.util.HashSet;

public class Day9 {
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
        var input = new java.io.File("inputs/day9.txt");
        var br = new BufferedReader(new FileReader(input));

        // Combines part one and two, head is [0], part one tail is [1], part two tails is [9]
        var rope = new Point[10];
        for (int i = 0; i < rope.length; i++) {
            rope[i] = new Point();
        }
        var head = rope[0];

        var visitedPartOneTail = new HashSet<String>();
        var visitedPartTwoTail = new HashSet<String>();

        String line;

        while ((line = br.readLine()) != null) {
            var parts = line.split(" ");
            var direction = parts[0];
            int distance = Integer.parseInt(parts[1]);

            for (int i = 0; i < distance; i++) {
                switch (direction) {
                    case "L" -> --head.x;
                    case "R" -> ++head.x;
                    case "U" -> ++head.y;
                    case "D" -> --head.y;
                }

                for (int j = 1; j < rope.length; j++) {
                    if (!rope[j].isAdjacentTo(rope[j - 1])) {
                        int xOffset = rope[j - 1].x - rope[j].x;
                        int yOffset = rope[j - 1].y - rope[j].y;

                        if (xOffset < 0)
                            --rope[j].x;
                        else if (xOffset > 0)
                            ++rope[j].x;

                        if (yOffset < 0)
                            --rope[j].y;
                        else if (yOffset > 0)
                            ++rope[j].y;
                    }

                    visitedPartOneTail.add(rope[1].toString());
                    visitedPartTwoTail.add(rope[9].toString());
                }
            }
        }

        System.out.println(visitedPartOneTail.size());
        System.out.println(visitedPartTwoTail.size());
    }

    static class Point {
        int x;
        int y;

        boolean isAdjacentTo(Point other) {
            return Math.abs(other.x - x) <= 1 && Math.abs(other.y - y) <= 1;
        }

        @Override
        public String toString() {
            return x + "," + y;
        }
    }
}