<style>

    @import url('https://fonts.googleapis.com/css2?family=Source+Code+Pro:wght@300&display=swap');
    @import url('https://fonts.googleapis.com/css2?family=Montserrat:wght@300&display=swap');

    * {

        font-family: 'Source Code Pro';
        font-size: 1.5rem;
        font-style: italic;
    }

    p {

        font-style: normal;
        font-family: 'Montserrat';
        letter-spacing: 1px;
        margin: 2rem 0;
    }

    h1 {

        font-size: 3rem;
        margin-top: 5rem;
    }

    h2 {

        font-size: 2rem;
        color: pink;
        margin: 6rem 0 5rem 0;
    }
</style>

# C# Concepts

## Class Properties

If we have a private data field in a class for data absraction, then to acces or modify that field, we need class properties. Class properties have getters and setters which are responsible for accessing or modifying the data field.

```cs

    class Person
    {
        private string name;

        // class properties

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = Name;
            }
        }
    }
```

Shorthand property :

```cs

    class Person
    {
        public string Name
        {
            get;
        }
    }
```

## Sealed Keyword

If we don't want other classes to inherit from one class, we use the 'sealed' keyword :

```cs

    sealed class Animal
    {
        void sleep ( )
        {
            Console.WriteLine("Sleeping");
        }
    }
```

## Abstraction

Abstract classes cannot be instantiated. This concept is mainly used for data-abstraction. Abstract classes have abstract functions, which don't have any bodies. Classes which inherit the abstract base class, inside them we override and define the bodies of the abstract functions.

```cs

    abstract class Animal
    {
        public abstract void animalSound( );

        public void sleep ( )
        {
            Console.WriteLine("Sleeping");
        }
    }

    class Pig: Animal
    {
        public override void animalSound ( )
        {
            Console.WriteLine("Wee");
        }
    }

    class Program
    {
        static void main ( )
        {
            Pig pig = new Pig( );

            pig.animalSound( );
            pig.sleep( );
        }
    }
```

## Interfaces

An abstract class, which has only abstract functions is called an interface.

```cs

    interface Animal
    {
        void sound( );
    }

    class Pig: Animal
    {
        void sound ( )
        {
            Console.WriteLine("Wee");
        }
    }
```

A class can inherit from multiple interfaces.

```cs

    interface Animal
    {
        void sound( );
    }

    interface Mammal
    {
        void countLegs( );
    }

    class Pig: Animal, Mammal
    {
        void sound ( )
        {
            Console.WriteLine("Wee");
        }

        void countLegs ( )
        {
            Console.WriteLine("Number of legs - 4");
        }
    }
```