import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.util.Arrays;
import java.util.stream.IntStream;

public class Day8 {
    public static void main(String[] args) throws IOException {
        long elapsedTimeSum = 0;
        for (int i = 0; i < 1000; i++) {
            long startTime = System.nanoTime();
            var map = parseTreeMap();
            calculateVisibility(map);
            calculateTreeScore(map);
            long elapsedTime = System.nanoTime() - startTime;
            elapsedTimeSum += elapsedTime;
        }
        System.out.println("average execution time (ns): " + elapsedTimeSum/1000);
    }

    private static char[][] parseTreeMap() throws IOException {
        var input = new java.io.File("inputs/day8.txt");
        var br = new BufferedReader(new FileReader(input));

        return br.lines().map(String::toCharArray).toArray(char[][]::new);
    }

    private static void calculateVisibility(char[][] map) {
        var visibilityMap = new boolean[map.length][map[0].length];

        // TODO: less ugly way of doing this
        for (int y = 0; y < map.length; y++) {
            int highestPosX = 0;
            for (int x = 0; x < map[0].length; x++) {
                if (map[x][y] > highestPosX) {
                    visibilityMap[x][y] = true;
                    highestPosX = map[x][y];
                }
            }

            int highestNegX = 0;
            for (int x = map[0].length - 1; x >= 0; x--) {
                if (map[x][y] > highestNegX) {
                    visibilityMap[x][y] = true;
                    highestNegX = map[x][y];
                }
            }
        }

        for (int x = 0; x < map[0].length; x++) {
            int highestPosY = 0;
            for (int y = 0; y < map.length; y++) {
                if (map[x][y] > highestPosY) {
                    visibilityMap[x][y] = true;
                    highestPosY = map[x][y];
                }
            }

            int highestNegY = 0;
            for (int y = map.length - 1; y >= 0; y--) {
                if (map[x][y] > highestNegY) {
                    visibilityMap[x][y] = true;
                    highestNegY = map[x][y];
                }
            }
        }

        var count = Arrays.stream(visibilityMap).parallel()
                .map(m -> IntStream.range(0, m.length)
                        .mapToObj(i -> m[i]))
                .mapToLong(s -> s.filter(v -> v)
                        .count())
                .sum();

        System.out.println(count);
    }

    private static void calculateTreeScore(char[][] map) {
        int highestTreeScore = 0;

        // TODO: less ugly way of doing this
        for (int y = 1; y < map[0].length - 1; y++) {
            for (int x = 1; x < map.length - 1; x++) {
                int offsetNegX = 1;
                while (x - (offsetNegX + 1) > -1 && map[x - offsetNegX][y] < map[x][y]) {
                    ++offsetNegX;
                }

                int offsetPosX = 1;
                while (x + (offsetPosX + 1) < map.length && map[x + offsetPosX][y] < map[x][y]) {
                    ++offsetPosX;
                }

                int offsetNegY = 1;
                while (y - (offsetNegY + 1) > -1 && map[x][y - offsetNegY] < map[x][y]) {
                    ++offsetNegY;
                }

                int offsetPosY = 1;
                while (y + (offsetPosY + 1) < map.length && map[x][y + offsetPosY] < map[x][y]) {
                    ++offsetPosY;
                }

                int score = offsetNegX * offsetPosX * offsetNegY * offsetPosY;
                if (score > highestTreeScore)
                    highestTreeScore = score;
            }
        }

        System.out.println(highestTreeScore);
    }
}