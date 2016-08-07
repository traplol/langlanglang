# Langlanglang

Langlanglang (lll) is a staticly typed language with some features similar to
dynamically typed languages mixed with C. Currently this lll compiler implementation
targets C mostly because of the semi-portability of C and the free optimizations
which come with any smart C compiler. As of this writing the front end of this
compiler targets Microsoft's CL.exe, but all of the examples shown have also been
tested by hand against GCC and Clang.


## Overview

As of this writing this Langlanglang compiler does not implement a garbage collector,
however the language will provide optional garbage collection which will be available
as a drop-in/out compiler flag and the ability to manually manage memory without the
garbage collector being aware of it.

### Hello world

```
$ cat hello-world.lll
printf("Hello world!\n");
$ lllc hello-world.lll
$ ./hello-world.exe
Hello world!
```

### Declaring variables

In Langlanglang you may omit the type declaration if the type may be infered from
the right hand side of the assignment.

```
var0: int;          // good, uninitialzed but declared as an integer.
var1 := 42;         // good, type is infered to be an integer
var2 : int = 123;   // good, type inference will check the declared type

// bad, the compiler will throw an error saying `var3' is undeclared.
var3 = 77;
```

### Branches
*NOTE:* The curly braces for any of these constructs are required and CANNOT be omitted.

Langlanglang has typical branching, the ```else if``` and ```else``` can be omitted.

```
if 1 {
   printf("True!");
}
else if 1 {
   printf("this doesn't print");
}
else {
   printf("this doesn't print either");
}
```

### Functions

```
func fib(n: i32) -> i32 {
    if n < 2 {
       return n;
    }
    return fib(n - 1) + fib(n - 2);
}
```

In the above example you may notice the ```-> i32```, this is the return type of
the function. You may omit this if you function doesn't return anything or you
can even be explicit with ```-> void```.


### Loops

#### ```while``` loop
```
while someCondition || another {
    printf("I'm working!");
}
```


#### ```for``` loop
```
for i := 0; i < 10; i = i + 1 {
    printf("%d, ", i);
}
```

#### ```for..in``` loop

```
for i in 0..10 {
    printf("%d, ", i)
}
```
The ```for..in``` loop is really just syntactic sugar of a normal ```for``` loop except
the range of the ```for..in``` loop is inclusive, it starts at one end of the range and
goes to the other. ```for..in``` loops also work with variables and to make the ```for..in```
loop run in reverse you must specify the ```-``` right before the opening curly brace.

```
N := 10;
for i in N..0 - {
    printf("%d, ", i)
}

for i in N/2..N*2 { // implicitly, the + operator is chosen if non is specified.
    printf("%d, ", i)
}
```

#### ```foreach..in``` loop (NOT YET IMPLEMENTED)
This loop will work to iterate on anything that implements a special iterator extension.

### Structures/Extensions

Like C and other languages, Langlanglang has the ability to define structures. Be aware
that Langlanglang is not meant to be an object oriented language and there is no intention
to have any form inheritance. 

```
struct Person {
    Age: i32;
    FirstName: char*;
    LastName: char*;
}

john: Person; // unitialized block of memory
john.Age = 36;
john.FirstName = "John";
john.LastName = "Doe";
```

The key feature of Langlanglang is it's ability to "extend" any structure with another
function.

*NOTE:* These "extensions" are really just static functions so they do not add any bloat
to structures.

```
// Here is a constructor/initializer for the Person struct.
extend Person(this, age: i32, first: char*, last: char*) {
    this.Age = age;
    this.FirstName = first;
    this.LastName = last;
}

jane := new Person(27, "Jane", "Smith");
printf("%s, %s: %d\n", jane.LastName, jane.FirstName, jane.Age); // Smith, Jane: 27
```

It would get pretty ugly to have that printf statement anywhere you wanted to print
a person's name and age, so let's implement an extension to do it instead.

```
extend Person print(this) { // note the void return
    printf("%s, %s: %d\n",
        this.LastName,
        this.FirstName,
        this.Age);
}

jane.print(); // this is much better.
john.print();
```

Extensions are really a key feature of Langlanglang, you can extend any structure
with any function so long as the function signature is unique, excluding return type.

### Operators

By default Langlanglang provides the standard operators for the primitive types such
as floats and ints; but if you want to allow an operator the be usable on a structure
you can do it the same way you would any other extension. The only difference is the
names of operator functions are reserved.


```
struct Vec3 {
    X: double;
    Y: double;
    Z: double;
}

extend Vec3(this, x: double, y: double, z: double) {
    this.X = x;
    this.Y = y;
    this.Z = z;
}

// here we will implement adding two vectors.
extend Vec3 __add__(this, other: Vec3*) -> Vec3* {
    x := this.X + other.X;
    y := this.Y + other.Y;
    z := this.Z + other.Z;
    return new Vec3(x, y, z);
}

v1 := new Vec3(1.0, 2.0, 3.0);
v2 := new Vec3(4.0, 5.0, 6.0);

v3 := v1 + v2;
// v3.X -> 5;
// v3.Y -> 7;
// v3.Z -> 9;

/* Let's extend the relational operators. */

// v1 == v2
extend Vec3 __eq__(this, other: Vec3*) -> bool {
    return this.X == other.X
      && this.Y == other.Y
      && this.Z == other.Z;
}

// v1 != v2
extend Vec3 __ne__(this, other: Vec3*) -> bool {
    // note this actually creates a call to this.__eq__(other);
    return !(this == other);
}

// v1 < v2
extend Vec3 __lt__(this, other: Vec3*) -> bool {
    return this.X < other.X
      && this.Y < other.Y
      && this.Z < other.Z;
}

// v1 <= v2
extend Vec3 __le__(this, other: Vec3*) -> bool {
    return this < other || this == other;
}

// v1 > v2
extend Vec3 __gt__(this, other: Vec3*) -> bool {
    return !(this <= other);
}

// v1 >= v2
extend Vec3 __ge__(this, other: Vec3*) -> bool {
    return this == other || this > other;
}
```

#### Operator aliases
````
Usage            Alias
 x[y]            x.__index__[y]
 x + y           x.__add__(y)
 x - y           x.__sub__(y)
 x * y           x.__mul__(y)
 x / y           x.__div__(y)
 x % y           x.__mod__(y)
 x & y           x.__and__(y)
 x ^ y           x.__xor__(y)
 x | y           x.__or__(y)
 x << y          x.__lshift__(y)
 x >> y          x.__rshift__(y)
 x < y           x.__lt__(y)
 x > y           x.__gt__(y)
 x <= y          x.__le__(y)
 x >= y          x.__ge__(y)
 x == y          x.__eq__(y)
 x != y          x.__ne__(y)
````

### Function Overloading
Another feature available in Langlanglang is the ability to overload function paramters,
the compiler will attempt to find the best match for the overloaded function and it will
fail compilation with an error when a call to an overloaded function is ambiguous.

```
func foo(a: int) {
    printf("%d\n", a);
}

func foo(a: char*) {
    printf("%s\n", a);
}

foo(42);
foo("hello world");
```


# TODO:
```
Document auto-references
Document generic functions
```