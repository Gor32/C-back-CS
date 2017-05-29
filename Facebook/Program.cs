using Facebook;
using System;

class Program
{
    static void Main()
    {
        string input = Console.ReadLine();
        string url = "https://graph.facebook.com/"+input+"/?fields=posts.limit(5){likes.summary(true)}&access_token=EAACEdEose0cBABO2cSTMvB0qPAmaIFXo2HHs1SYc0ncbhPqouwV42y1dYCpY43VOzhk4062qx28I5ENUnOHnkgfuM4ILysrFQZAOo8ZCl659Nh8U7F8S8mxq0JZCyfvtWHbk83jme8BAxHICpxtgch2SXzTOqVFKvyyvX80fwxBZAcr14VTN4XXDHCIVc2cZD";
        FacebookLike.PrintLikeCount(url);
    }
}