<text id="markdown">
class: centre, middle
# Behavioural Testing

---

#Intro
Explain behaviour vs integration, classical vs mockist, chicago vs london
---
#Case study
We are developing a system for a publishing house. Our *Reviews-API* will allow us to receive reviews about our books and store them.
The request uses an industry standard API, an example follows

```http
Content-type: application/json
Referer: https://literaryreview.co.uk/
```
```json
{
    "whenPosted" : "2017-04-03T18:25:43.511Z",
    "book": {
        "isbn": "9788175257665",
        "title": "War and Peace"
    },
    "review": {
        "reviewer": "Karen Castle",
        "score": 87,
        "content": "blah blah blah"
    }
}
```
---
#Requirements
* The `Content-type` must be `application/json`
* The `Referer` must have a valid URI format
* Each `isbn` must be 13 numeric characters
* Each `score` must be a value between 0 and 100 inclusive.
* Each valid review will be saved to our database. The database service has the following interface:

```c#
public interface ISaveReviews
{
    void Insert(ReviewDto dto);
}

public class ReviewDto
{
    public long ISBN { get; set; }
    public string Reviewer { get; set; }
    public string Uri { get; set; }
    public int Score { get; set; }
}
```
---
#Bad example
Module or controller or handler
Validation: header and dto validation
Mapping
Persistence
Both validation and mapping use a shared graph walker
Tests have everything mocked
---
#Good example
Only mock persistence
Use builder
---
#What's don't I like about 1st example
* Test readability
* Tight coupling to code
* Difficult to re factor
* Too many tests
---
#What's worse about behavioural testing
* Test case organisation
    * Fix with feature folder
    * Fix with coverage tool
* Cascading test failures
    * No fix but is this such a bad thing?
* Tight coupling to constructors
    * Fix with builders
---
#That's only half the story.
All I did was take a sut designed in mocks style and improve tests to a behavioural style
TDD is about design at least as much as testing
How you go about TDD has a direct effect on the design if your code
If you mostly test interactions, you'll get more interactions.
If you mostly test behaviours, you'll get more behaviours.
Now start from scratch and do TDD in fully behavioural style.
---
</text>