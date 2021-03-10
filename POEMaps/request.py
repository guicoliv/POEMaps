import sys
import json
import cloudscraper


def postforfirst100forMap(mapCode):
    #print(mapCode)
    url = 'https://www.pathofexile.com/api/trade/exchange/Ritual'
    myobj = {"exchange":{"status":{"option":"online"},"have":["chaos"],"want":[mapCode]}}
    headers = {"Content-Type": "application/json"}


    scraper = cloudscraper.create_scraper()
    #result = json.loads()
    print(scraper.post(url, headers=headers, data=json.dumps(myobj)).text)
    '''
    print('id:'+result['result'][0])


    geturl = 'https://www.pathofexile.com/api/trade/fetch/'+result['result'][0]
    getresult = json.loads(scraper.get(geturl, headers=headers).text)
    return getresult['result'][0]['item']['properties'][1]['values'][0][0]
    '''

def get100forMap(codes):
    print(codes)

if __name__ == '__main__':
    #print(sys.argv[0])
    #print(sys.argv[1])
    
    if str(sys.argv[2]) == "post":
        postforfirst100forMap(sys.argv[1])    
    if str(sys.argv[2]) == "get":
        get100forMap(sys.argv[1])
    


    #print("done")