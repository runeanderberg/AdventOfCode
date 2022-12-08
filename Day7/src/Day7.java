import java.io.*;
import java.util.ArrayList;
import java.util.List;

public class Day7 {
    public static void main(String[] args) throws IOException {
        long elapsedTimeSum = 0;
        for (int i = 0; i < 100000; i++) {
            long startTime = System.nanoTime();
            var root = parseTerminalOutput();
            solvePartOne(root);
            solvePartTwo(root);
            long elapsedTime = System.nanoTime() - startTime;
            elapsedTimeSum += elapsedTime;
        }
        System.out.println("average execution time (ns): " + elapsedTimeSum/100000);
    }

    private static Directory parseTerminalOutput() throws IOException {
        var input = new java.io.File("inputs/day7.txt");
        var br = new BufferedReader(new FileReader(input));

        var root = new Directory("/", null);
        var current = root;
        String line;

        while ((line = br.readLine()) != null) {
            if (line.charAt(0) == '$') {
                // if ls command we can just start parsing next line
                if (line.charAt(2) == 'l')
                    continue;

                // we have cd'ed, move to the correct directory
                if (line.charAt(5) == '/') {
                    current = root;
                } else if (line.charAt(5) == '.') {
                    current = current.getParent();
                } else {
                    var parts = line.split(" ");
                    current = (Directory) current.findChild(parts[2]);
                }

                continue;
            }

            if (line.charAt(0) == 'd') {
                current.addChild(new Directory(line.substring(4), current));
            } else {
                var parts = line.split(" ");
                current.addChild(new File(parts[1], Integer.parseInt(parts[0])));
            }
        }

        return root;
    }

    private static void solvePartOne(Directory root) {
        System.out.println(sum(root));
    }

    private static int sum(Directory dir) {
        int sum = 0;

        if (dir.size() <= 100000)
            sum += dir.size();

        sum += dir.getChildren().stream()
                .filter(Item::isDirectory)
                .mapToInt(d -> sum((Directory) d))
                .sum();

        return sum;
    }

    private static void solvePartTwo(Directory root) {
        var totalUsage = root.size();
        var target = 0;

        if (totalUsage > 70000000 - 30000000) {
            target = totalUsage - (70000000 - 30000000);
        }

        System.out.println(findDelete(root, target, Integer.MAX_VALUE));
    }

    private static int findDelete(Directory dir, int target, int smallest) {
        var smallestInDir = dir.getChildren().stream()
                .filter(Item::isDirectory)
                .mapToInt(Item::size)
                .filter(s -> s >= target && s < smallest)
                .sorted().findFirst().orElse(Integer.MAX_VALUE);

        var smallestAmongChildren = dir.getChildren().stream()
                .filter(Item::isDirectory)
                .mapToInt(d -> findDelete((Directory) d, target, smallest))
                .sorted().findFirst().orElse(Integer.MAX_VALUE);

        return Math.min(Math.min(smallestInDir, smallestAmongChildren), smallest);
    }
}

interface Item {
    String name();
    int size();
    boolean isDirectory();
}

record File(String name, int size) implements Item {
    @Override
    public boolean isDirectory() {
        return false;
    }
}

class Directory implements Item {
    private final String name;
    private final Directory parent;
    private final List<Item> children;

    public Directory(String name, Directory parent) {
        this.name = name;
        this.parent = parent;
        children = new ArrayList<>();
    }

    @Override
    public String name() {
        return name;
    }

    @Override
    public int size() {
        return children.stream()
                .mapToInt(Item::size)
                .sum();
    }

    @Override
    public boolean isDirectory() {
        return true;
    }

    public Directory getParent() {
        return parent;
    }

    public void addChild(Item child) {
        this.children.add(child);
    }

    public Item findChild(String name) {
        return children.stream()
                .filter(c -> c.name().equals(name))
                .findFirst()
                .orElse(null);
    }

    public List<Item> getChildren() {
        return children;
    }
}