#import "CommonFunction.h"
#import <sys/xattr.h>
#import <AdSupport/AdSupport.h>

#include <sys/socket.h>
#include <netdb.h>
#include <arpa/inet.h>
#include <err.h>

#define MakeStringCopy2( _x_ ) ( _x_ != NULL && [_x_ isKindOfClass:[NSString class]] ) ? strdup( [_x_ UTF8String] ) : NULL

@implementation CommonFunction

#ifdef __cplusplus
extern "C" {
#endif

	char* MakeStringCopy(const char* s)
	{
		if (s == NULL)
			return NULL;
		
		char* res = (char*)malloc(strlen(s) + 1);
		strcpy(res, s);
		return res;
	}
	
	char* GetBundleShortVersion()
	{
		return MakeStringCopy([[[NSBundle mainBundle] objectForInfoDictionaryKey:@"CFBundleShortVersionString"] UTF8String]);
	}
	
	char* GetBundleVersion()
	{
		return MakeStringCopy([[[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleVersion"] UTF8String]);
	}
	
	void AddSkipBackupAttributeToItemAtURL(NSURL* URL)
	{
		const char* filePath = [[URL path] fileSystemRepresentation];
		const char* attrName = "com.apple.MobileBackup";
		u_int8_t attrValue = 1;
		setxattr(filePath, attrName, &attrValue, sizeof(attrValue), 0, 0);
	}
	
	void SetNotBackup()
	{
		NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
		NSString *docDir = [paths objectAtIndex:0];
		NSString* temp = [docDir stringByAppendingFormat:@"/NotBackup"];
		[[NSFileManager defaultManager] createDirectoryAtPath:temp withIntermediateDirectories:YES attributes:nil error:nil];
		NSURL *dbURLPath = [NSURL URLWithString:temp];
		AddSkipBackupAttributeToItemAtURL(dbURLPath);
	}
	
	void GotoAppStore(char* url)
	{
		NSString *str = [NSString stringWithUTF8String:url];
		[[UIApplication sharedApplication] openURL:[NSURL URLWithString:str]];
	}

	char* GetIDFA()
	{
		NSString *adId = [[[ASIdentifierManager sharedManager] advertisingIdentifier] UUIDString];
		return MakeStringCopy([adId UTF8String]);
	}

	char* GetIDFV()
	{
		NSString *idfv = [[[UIDevice currentDevice] identifierForVendor] UUIDString];
		return MakeStringCopy([idfv UTF8String]);
	}

	const char* GetIPv6(const char *mHost)
	{
		if( nil == mHost )
			return NULL;
		const char *newChar = "No";
		const char *cause = NULL;
		struct addrinfo* res0;
		struct addrinfo hints;
		struct addrinfo* res;
		int n, s;
		
		memset(&hints, 0, sizeof(hints));
		
		hints.ai_flags = AI_DEFAULT;
		hints.ai_family = PF_UNSPEC;
		hints.ai_socktype = SOCK_STREAM;
		
		if((n=getaddrinfo(mHost, "http", &hints, &res0))!=0)
		{
			printf("getaddrinfo error: %s\n",gai_strerror(n));
			return NULL;
		}
		
		struct sockaddr_in6* addr6;
		struct sockaddr_in* addr;
		NSString * NewStr = NULL;
		char ipbuf[32];
		s = -1;

		for(res = res0; res; res = res->ai_next)
		{
			if (res->ai_family == AF_INET6)
			{
				addr6 =( struct sockaddr_in6*)res->ai_addr;
				newChar = inet_ntop(AF_INET6, &addr6->sin6_addr, ipbuf, sizeof(ipbuf));
				NSString * TempA = [[NSString alloc] initWithCString:(const char*)newChar encoding:NSASCIIStringEncoding];
				NSString * TempB = [NSString stringWithUTF8String:"&&ipv6"];
				
				NewStr = [TempA stringByAppendingString: TempB];
				printf("%s\n", newChar);
			}
			else
			{
				addr =( struct sockaddr_in*)res->ai_addr;
				newChar = inet_ntop(AF_INET, &addr->sin_addr, ipbuf, sizeof(ipbuf));
				NSString * TempA = [[NSString alloc] initWithCString:(const char*)newChar encoding:NSASCIIStringEncoding];
				NSString * TempB = [NSString stringWithUTF8String:"&&ipv4"];
				
				NewStr = [TempA stringByAppendingString: TempB];			
				printf("%s\n", newChar);
			}
			break;
		}
	
	
		freeaddrinfo(res0);
		
		printf("getaddrinfo OK");
		
		NSString * mIPaddr = NewStr;
		return MakeStringCopy2(mIPaddr);
	}

#ifdef __cplusplus
}
#endif

@end