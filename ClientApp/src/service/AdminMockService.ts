
import api from './api';

export class AdminMockService {

  getMocks = async (typeMock:string): Promise<any> => {
    
      try {
        const { data } = await api.get<any[]>('/mocks/'+typeMock);
        return data;
      } catch(error: any) {
        if (error.response) {
          // The request was made. server responded out of range of 2xx
          alert(error.response.data.message);
        } else if (error.request) {
          // The request was made but no response was received
          alert('ERROR_NETWORK');
        } else {
          // Something happened in setting up the request that triggered an Error
          alert(error.message);
        }
    }
  }
  getMock = async (typeMock:string, name: string): Promise<any> => {
    let { data } = await api.get('/mocks/'+typeMock +'/' + name);
    return data;
  }
  newMock = (typeMock:string, name: string): any => {
    return {"someData":"someValue"};
  }
  saveMock = async (typeMock:string, name: string|null, template: any): Promise<any> => {
    api.post('/mocks/'+typeMock +'/' + name, template).then(response => {
    }).catch(error => {
      alert(error.message);
    })
  }
  deleteMock = async (typeMock:string, name: string): Promise<any> => {
    api.delete('/mocks/'+typeMock +'/' + name).then(response => {
    }).catch(error => {
      alert(error.message);
    })
  }
}

const adminMockService = new AdminMockService();

export default adminMockService;
