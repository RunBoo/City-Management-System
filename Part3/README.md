# Linked lists

For these questions you will be making a Linked List template class.

Note that in answering these questions, **you should not add any `#include` or `using` statements**: you must implement the functionality yourself, without using any data structures from the standard library.  I have added `#include <iostream>` for if you want to print things out to help your debugging.  If you have a convincing need for adding a different `#include` please post in the forum on KEATS.

Make sure you don't commit any compiled code to your GitHub repository; or if you choose to use an IDE, any large project directories created by your IDE.  You can make these on your machine, but don't `commit` or `add` them to your repository -- this isn't what git is designed for.

----

# LinkedList basics [3 marks]

## a) Making a list node

In the file `node.h` implement a template class `Node` that represents a node in a doubly-linked list.  It should have three `public` member variables:

- The `data` stored in that Node.  The type of this should be a template argument.
- A pointer to the `next` Node in the list
- A pointer to the `previous` Node in the list

Make a constructor that takes an item of data, stores it in the node, and sets the two pointers to `nullptr`.

## b) Making an iterator for list nodes

The file `node.h` contains an incomplete `NodeIterator` class, that contains a pointer to a `Node`.  It is a template class -- a `NodeIterator<T>` object contains a pointer to a `Node<T>`.

Complete the definition of the `NodeIterator` class, so that:

- We can increment the iterator, using ++, which makes it point to the next node in the list
- We can see if two iterators are the same, using the == operator.  Two iterators are the same if they point to the same Node.
- We can see if two iterators are different, using the != operator


To test your code, compile and run TestNode.cpp.  A Makefile has been provided, run:

`make TestNode`  

...at the command line.  This makes two Nodes, one linked to the other, then makes an iterator over these.

If you don't have make, you can always open the Makefile and copy-paste the command that will be used to compile TestNode:

`g++ -Wall -g -std=c++11 -o TestNode TestNode.cpp`

----

# Making a LinkedList class [6 marks]

In the file `linkedlist.h` implement a template class LinkedList.  This should use the `Node` class you have written so far.  As member variables you should have:

- A pointer to the head of the list
- A pointer to the tail of the list
- A count of how many elements are in the list

The functions in LinkedList should be:

- A constructor, that creates an empty list (head and tail are `nullptr`, 0 elements in the list)
- A `push_front` function that takes an item and pushes it onto the front of the list, i.e. it becomes the head of the list.  (Note this should *not* take a Node: for a LinkedList<T>, we should be able to pass a T to push_front.)
- A `front()` function, that returns a reference to the data inside the head node
- A `push_back` function that takes an item and pushes it onto the back of the list, i.e. it becomes the tail
- A `back()` function, that returns a reference to the data inside the tail node
- A `size()` function that returns the count of how many elements are in the list
- A `begin()` function that returns an iterator pointing to the head of the list
- An `end()` function that returns an iterator pointing to `nullptr`  (NB it doesn't point to the tail: it points *off the end* of the list -- and the Node after the tail is `nullptr`.)
- A destructor, that `delete`s every node in the list.
- A `reverse()`function that reverses the order of the nodes in the list (NB it doesn't make new nodes, it should re-order the existing nodes.)

(It is recommended you implement them in roughly this order.)

To test your LinkedList at this point, compile and run TestList.cpp.  You can do this using:

`make TestList`  

It may help you to comment out tests that use code you haven't written yet (as they won't compile).  Alternatively, if you push your code to github, the automated tests will be run individually and should work even if you have not completed all parts.

Note: when your work is marked, deductions will be made for memory leaks.  Take care to ensure that you have deleted every object you created with new, exactly once.

Once you're happy with your work so far, it's time to make the class a bit more useful.

First, extend your class to have a constructor that takes a [std::initializer\_list<T>](http://en.cppreference.com/w/cpp/utility/initializer_list) as its argument, and makes a list containing these.  You can `#include <initializer_list>` for this.  This will allow lists to be created using the handy syntax:

`LinkedList<int> numbers{4,6,8,10,12};`  

When we write this, the numbers in braces are put in the initializer_list and passed to the constructor.

Next, inserting and erasing elements from linked lists is a classic interview question.  Add functionality as follows:

- `insert()`, taking as arguments a NodeIterator pointing to where to insert the new element in the list; and the element itself.  The element should be inserted in this position, and an iterator to the new element returned.  For instance for the list:

`[3,5,7]`

...if we have an iterator pointing to `5` and call insert with this and `4` the new list would be:

`[3,4,5,7]`

...and insert() would return an iterator pointing to 4.

Note you may edit your iterator class to provide a function to access the Node pointer from inside the iterator.

- `erase()`, taking as argument a NodeIterator pointing to an element to erase; and returning what is now in its place -- after removing element *i*, it should return a NodeIterator to what is now element *i*.  For instance, if the list was:
 

`[3,5,7]`

...and an iterator pointing to `5` was erased, that would update the list to be:

`[3,7]`

...and return an iterator pointing to 7.  If you are deleting the last item off the list, it should return an iterator pointing to nullptr.

Again, as above, make sure your implementation avoids memory leaks.

- Additional overloads of `begin()` and `end()` that are const methods -- i.e. can be used on a `const LinkedList`, to iterate through and provide read-only access to the elements.  To go with this, you will need to make another iterator class with a different name of your choosing.

To test your LinkedList now, compile and run TestListD.cpp.  You can do this using:

`make TestListD`  
