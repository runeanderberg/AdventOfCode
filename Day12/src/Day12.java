import java.awt.*;
import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;

public class Day12 {
    public static void main(String[] args) throws IOException {
        long elapsedTimeSum = 0;
        for (int i = 0; i < 1000; i++) {
            long startTime = System.nanoTime();

            var map = new Map(parseInput());
            calculatePartOne(map);

            long elapsedTime = System.nanoTime() - startTime;
            elapsedTimeSum += elapsedTime;
        }
        System.out.println("average execution time (ns): " + elapsedTimeSum / 1000);
    }

    private static char[][] parseInput() throws IOException {
        var input = new java.io.File("inputs/day12.txt");
        var br = new BufferedReader(new FileReader(input));

        return br.lines().map(String::toCharArray).toArray(char[][]::new);
    }

    private static void calculatePartOne(Map map) {
        System.out.println(step(map, map.start.x, map.start.y, 0));
    }

    private static int step(Map map, int x, int y, int steps) {
        // If outside of map or already visited in fewer steps
        if (!map.validCoordinate(x, y) || map.getDistanceTo(x, y) <= steps)
            return Integer.MAX_VALUE;

        map.setDistanceTo(x, y, steps);

        // If this is end, done
        if (x == map.end.x && y == map.end.y)
            return 0;

        // Check neighbours
        int posXRes = Integer.MAX_VALUE;
        int negYRes = Integer.MAX_VALUE;
        int posYRes = Integer.MAX_VALUE;
        int negXRes = Integer.MAX_VALUE;

        if (map.validCoordinate(x + 1, y) && map.traversable(x, y, x + 1, y)) {
            posXRes = step(map, x + 1, y, steps + 1);
        }

        if (map.validCoordinate(x - 1, y) && map.traversable(x, y, x - 1, y)) {
            negXRes = step(map, x - 1, y, steps + 1);
        }

        if (map.validCoordinate(x, y + 1) && map.traversable(x, y, x, y + 1)) {
            posYRes = step(map, x, y + 1, steps + 1);
        }

        if (map.validCoordinate(x, y - 1) && map.traversable(x, y, x, y - 1)) {
            negYRes = step(map, x, y - 1, steps + 1);
        }

        var min = Math.min(Math.min(posXRes, negXRes), Math.min(posYRes, negYRes));

        return min == Integer.MAX_VALUE ? min : min + 1;
    }
}

class Map {
    private final char[][] heightMap;
    private final int[][] distanceMap;

    public final int sizeX;
    public final int sizeY;

    public Point start;
    public Point end;

    public Map(char[][] heightMap) {
        this.heightMap = heightMap;
        this.sizeX = heightMap.length;
        this.sizeY = heightMap[0].length;
        this.distanceMap = new int[sizeX][sizeY];

        for (int y = 0; y < heightMap.length; y++) {
            for (int x = 0; x < heightMap[0].length; x++) {
                if (heightAt(x, y) == 'S') {
                    start = new Point(x, y);
                } else if (heightAt(x, y) == 'E') {
                    end = new Point(x, y);
                }
                distanceMap[y][x] = Integer.MAX_VALUE;
            }
        }

        heightMap[start.y][start.x] = 'a';
        heightMap[end.y][end.x] = 'z';
    }

    public char heightAt(int x, int y) {
        return heightMap[y][x];
    }

    public int getDistanceTo(int x, int y) {
        return distanceMap[y][x];
    }

    public void setDistanceTo(int x, int y, int distance) {
        distanceMap[y][x] = distance;
    }

    public boolean traversable(int currentX, int currentY, int targetX, int targetY) {
        return heightDifference(currentX, currentY, targetX, targetY) <= 1;
    }

    public int heightDifference(int x1, int y1, int x2, int y2) {
        return heightAt(x2, y2) - heightAt(x1, y1);
    }

    public boolean validCoordinate(int x, int y) {
        return x >= 0 && y >= 0 && x < sizeY && y < sizeX;
    }
}