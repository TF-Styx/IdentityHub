import { createApiClient } from '@/shared/api/createApiClient';
import { BFF_URL } from '@/app/config';

const api = createApiClient(BFF_URL);

import type { 
    VerifySRPRequest, 
    GetSRPRequest, 
    GetSRPResponse,
    VerifySRPResponse
} from '@/features/auth/model/types';

export const getSRPChallenge = (login: GetSRPRequest) =>
  api.post<GetSRPResponse>('/auth/srp/challenge', login);

export const verifySRPProof = (data: VerifySRPRequest) =>
  api.post<VerifySRPResponse>('/auth/srp/verify', data);

export const logout = () => api.postVoid('/auth/logout', {});
