/**
 * TODO: Add your file header
 * Name:
 * ID:
 * Email:
 * Sources used: Put "None" if you did not have any external help
 * Some example of sources used would be Tutors, Zybooks, and Lecture Slides
 * 
 * 2-4 sentence file description here
 */

 //IMPORTANT: Do not change the headers!

import static org.junit.Assert.*;

import org.junit.*;


/**
 * TODO: Add your class header (1-2 sentences)
 */
public class MyArrayListHiddenTester {
	static final int DEFAULT_CAPACITY = 5;
	static final int MY_CAPACITY = 3;

	Object[] arr = new Object[10];
	Integer[] arrInts = { 1, 2, 3 };

	private MyArrayList listEmpty, listDefaultCap, listCustomCapacity, 
		listWithNull, listWithInt;
	/**
	 * This sets up the test fixture. JUnit invokes this method before
	 * every testXXX method. The @Before tag tells JUnit to run this method
	 * before each test */
	@Before
	public void setUp() throws Exception {
		listEmpty = new MyArrayList();
		listDefaultCap = new MyArrayList(DEFAULT_CAPACITY);
		listCustomCapacity = new MyArrayList(MY_CAPACITY);
		listWithNull = new MyArrayList(arr);
		listWithInt = new MyArrayList<Integer>(arrInts);
	}

	/**
	 * Aims to test the constructor when the input argument
	 * is not valid
	 */
	@Test
	public void testConstructorInvalidArg(){
		Boolean exceptionThrown = false;
		try {
			MyArrayList testArray = new MyArrayList(-1);
		}
		catch (IllegalArgumentException exception){
			exceptionThrown = true;
		}
		assertTrue("Exception not thrown for invalid argument", exceptionThrown);
	}

	/**
	 * Aims to test the constructor when the input argument
	 * is null
	 */
	@Test
	public void testConstructorNullArg(){
		Boolean exceptionThrown = false;
		arr = null;
		try {
			MyArrayList testArray = new MyArrayList(arr);
		}
		catch (Exception exception) {
			exceptionThrown = true;
		}

		MyArrayList testArray = new MyArrayList(arr);
		assertFalse("Exception thrown for null argument", exceptionThrown);
		assertEquals(0, testArray.size());
		assertEquals(5, testArray.getCapacity());
	}

	/**
	 * Aims to test the append method when an element is added to a full list
	 * Check reflection on size and capacity
	 */
	@Test
	public void testAppendAtCapacity(){
		Boolean exceptionThrown = false;
		try {
			listWithInt.append(4);
		}
		catch (IndexOutOfBoundsException exception) {
			exceptionThrown = true;
		}
		assertEquals("Check capacity", 6, listWithInt.getCapacity());
		assertEquals("Check size", 4, listWithInt.size());
		assertFalse("Exception thrown for appending at capacity", exceptionThrown);
	}

	/**
	 * Aims to test the prepend method when a null element is added
	 * Checks reflection on size and capacity
	 * Checks whether null was added successfully
	 */
	@Test
	public void testPrependNull(){
		Boolean exceptionThrown = false;
		try {
			listWithInt.prepend(null);
		}
		catch (Exception exception) {
			exceptionThrown = true;
		}
		assertFalse("Exception thrown for prepending", exceptionThrown);
		assertEquals(6, listWithInt.getCapacity());
		assertEquals(4, listWithInt.size());
		
	}
	
	/**
	 * Aims to test the insert method when input index is out of bounds
	 */
	@Test
	public void testInsertOutOfBound(){
	   Boolean exceptionThrown = false;
	   try {
		   listWithInt.insert(3, 4);
	   }
	   catch (IndexOutOfBoundsException exception) {
		   exceptionThrown = true;
	   }
	   assertTrue("Exception not thrown for inserting out of bounds", exceptionThrown);
	}

	/**
	 * Insert multiple (eg. 1000) elements sequentially beyond capacity -
	 * Check reflection on size and capacity
	 * Hint: for loop could come in handy
	 */
	@Test
	public void testInsertMultiple(){
		Boolean exceptionThrown = false;
		try {
			for (int i = 0; i < 1000; i++) {
				listWithInt.insert(i, i);
			}
		}
		catch (Exception exception) {
			exceptionThrown = true;
		}
		assertFalse("Exception is thrown", exceptionThrown);
		assertEquals(1536, listWithInt.getCapacity());
		assertEquals(1003, listWithInt.size());
	}

	/**
	 * Aims to test the get method when input index is out of bound
	 */
	@Test
	public void testGetOutOfBound(){
		Boolean exceptionThrown = false;
		try {
			listWithInt.get(5);
		}
		catch (IndexOutOfBoundsException exception) {
			exceptionThrown = true;
		}
		assertTrue("Exception is not thrown", exceptionThrown);
	}

	/**
	 * Aims to test the set method when input index is out of bound
	 */
	@Test
	public void testSetOutOfBound(){
		Boolean exceptionThrown = false;
		try {
			listWithInt.set(5, 4);
		}
		catch (IndexOutOfBoundsException exception) {
			exceptionThrown = true;
		}
		assertTrue("Exception is not thrown", exceptionThrown);
	}


	/**
	 * Aims to test the remove method when input index is out of bound
	 */
	@Test
	public void testRemoveOutOfBound(){
		Boolean exceptionThrown = false;
		try {
			listWithInt.remove(5);
		}
		catch (IndexOutOfBoundsException exception) {
			exceptionThrown = true;
		}
		assertTrue("Exception is not thrown", exceptionThrown);
	}

	/**
	 * Aims to test the expandCapacity method when 
	 * requiredCapacity is strictly less than the current capacity
	 */
	@Test
	public void testExpandCapacitySmaller(){
		Boolean exceptionThrown = false;
		try {
			listWithInt.expandCapacity(1);
		}
		catch (IllegalArgumentException exception) {
			exceptionThrown = true;
		}
		assertTrue("Exception is not thrown", exceptionThrown);
	}

	/**
	 * Aims to test the expandCapacity method when 
	 * requiredCapacity is greater than double(2x) the current capacity
	 */
	@Test
	public void testExpandCapacityExplode(){
		Boolean exceptionThrown = false;
		try {
			listWithInt.expandCapacity(100);
		}
		catch (Exception exception) {
			exceptionThrown = true;
		}
		assertFalse("Exception thrown", exceptionThrown);
		assertEquals(100, listWithInt.getCapacity());
	}
	

}
