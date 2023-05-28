import numpy as np
import math
import random
import time

class State:
    distances = []
    cityList = range(0)
    
    def __init__(self, actualCity, notVisited, visited):
        self.actualCity = actualCity
        self.notVisited = notVisited
        self.visited = visited
        
        self.g = 0  # vzdalenost mezi startem a aktualnim uzlem
        self.h = 0  # heuristicka informace
        self.f = 0  # ohodnoceni

    def heuristic(self, home_state):
        closest = self.findCloses(home_state,False)
        mst = self.MST()
        home = self.findCloses(home_state,True)
        h = closest + mst + home
        return h

    def findCloses (self,home_state,use_hs):
        minimal = float('inf')
        unvisited = self.notVisited.copy()
        
        if use_hs==True:
            considered_city = home_state.actualCity
        if use_hs==False:
            considered_city = self.actualCity

        for x in unvisited:
            if x == considered_city:
                continue
            if State.distances[x,considered_city] < minimal:
                minimal = State.distances[x,considered_city]
        return minimal

    def MST(self):
        # create sub-SortedDistances from the nodes that hasnt been traveled yet
        result = []
        graph = []
        i, e = 0, 0

        graph_list = self.notVisited.copy()
        graph_list.append(self.actualCity)
        graph_list.sort()
        inx = -1
        for ix in graph_list:
            inx += 1
            jnx = -1
            for jx in graph_list:
                jnx += 1
                if ix < jx:
                    graph.append( [inx, jnx, State.distances[ix,jx]] )
        graph.sort(key=lambda item: item[2])
       

        parent = []
        rank = []
        for node in range(len(self.notVisited)+1):
            parent.append(node)
            rank.append(0)
        while e < len(graph_list)-1:
            u, v, w = graph[i]
            i = i + 1
            x = graph_find(parent, u)
            y = graph_find(parent, v)
            if x != y:
                e = e + 1
                result.append([u, v, w])
                graph_apply_union(parent, rank, x, y)
        
        col = [tple[2] for tple in result]

        return sum(col)


    def expand_astar(self, state_inx):
        
        cost = State.distances[self.actualCity, state_inx]
        
        new_visited = self.visited.copy()
        new_visited.append(self.actualCity)
        new_notVisited = self.notVisited.copy()
        new_notVisited.remove(state_inx)

        new_state = State(state_inx, new_notVisited, new_visited)

        return (new_state, cost)

    def compare(self, state):
        if (
            self.actualCity == state.actualCity
            and self.visited.copy() == state.visited.copy()
            ):
            return True
        else:
            return False

    def isFinal(self):
        visited_and_actual = self.visited.copy()
        visited_and_actual.append(self.actualCity)
        if (
            sorted(visited_and_actual) == sorted([*range(0,int(math.sqrt(State.distances.size)))])
            ):
            return True
        else:
            return False


def graph_find(parent, i):
    if parent[i] == i:
        return i
    return graph_find(parent, parent[i])

def graph_apply_union(parent, rank, x, y):
        xroot = graph_find(parent, x)
        yroot = graph_find(parent, y)
        if rank[xroot] < rank[yroot]:
            parent[xroot] = yroot
        elif rank[xroot] > rank[yroot]:
            parent[yroot] = xroot
        else:
            parent[yroot] = xroot
            rank[xroot] += 1

def exists(que, new_state):

    for x in que:
        if x.compare(new_state):
            return True

    return False

def astar(home_state):

    start_state = home_state

    open = list()
    closed = list()
    solution = None
    found = False
    path = None

    expanded_nodes = 0
    gen_nodes = 0
    
    if State.distances.size==1:
        print("Solution found, no cities to travel!")
        return [start_state]

    # musime spocitat heuristiku pro startovni stav a vlozit jej do OPEN
    start_state.h = start_state.heuristic(start_state)
    start_state.f = 0 + start_state.h  # g hodnota je 0, jsme na startu
    
    open.append(start_state)

    while(open):
        
        open.sort(key=lambda state: state.f)
        state = open.pop()
        
        if found == True and state.f>=solution.f:
            break

        expanded_nodes += 1
        closed.append(state)

        for i in state.notVisited:
            (new_state, cost) = state.expand_astar(i)

            if not new_state:
                continue
            
            gen_nodes += 1

            if new_state.isFinal():
                solution = new_state
                found = True
                break

            if not exists(open, new_state) and not exists(closed, new_state):
                new_state.g = state.g + cost
                new_state.h = new_state.heuristic(start_state)
                new_state.f = new_state.g + new_state.h
                open.append(new_state)
            
    if found:
        # solution found
        print("--------------------------")
        print("Total expanded nodes: ", expanded_nodes)
        print("Total generated nodes: ", gen_nodes)
        path = solution.visited.copy()
        path.append(solution.actualCity)
        path.append(home_state.actualCity)

    return path



if __name__ == "__main__":
    
    ## Define distances - option 1.:
    #===============================     
    # Distances between nodes:
    number_of_nodes = 5
    d = np.zeros((number_of_nodes,number_of_nodes))
    d[0,1] = 4.12
    d[0,2] = 6.00
    d[0,3] = 4.47
    d[0,4] = 3.00
    
    d[1,2] = 2.24
    d[1,3] = 3.00
    d[1,4] = 1.41

    d[2,3] = 2.82
    d[2,4] = 3.00

    d[3,4] = 2.24

    
    ## Define distances - option 2.:
    #===============================
    # In nodes distances in matrix:
    #d = [   [0.00, 4.12, 6.00, 4.47, 3.00],
    #        [0.00, 0.00, 2.24, 3.00, 1.41],
    #        [0.00, 0.00, 0.00, 2.82, 3.00],
    #        [0.00, 0.00, 0.00, 0.00, 2.24],
    #        [0.00, 0.00, 0.00, 0.00, 0.00]
    #    ]
    #number_of_nodes = int(math.sqrt(State.distances.size))


    ## Define distances - option 3.:
    #===============================
    # Random travel distance for time testing: 
    #number_of_nodes = 200
    #d = np.zeros((number_of_nodes,number_of_nodes))
    #for inx in range(number_of_nodes):
    #    for jnx in range(inx+1,number_of_nodes):
    #        d[inx,jnx] = random.randrange(1, 100)*0.1


    d = d + np.transpose(d)
    State.distances = d

    # start_index ... index of starting city
    start_index = 0

    notVis = [*range(0,int(math.sqrt(State.distances.size)))]
    notVis.remove(start_index)
    start = State(start_index, notVis,[])

    # 
    init = time.perf_counter()
    path = astar(start)
    tot_time = time.perf_counter()-init

    inxs_taveled = []
    tot_len = 0

    if path:
        print("--------------------------")
        print("Solution found!")
        for x in path:
            inxs_taveled.append(x)
            if len(inxs_taveled)>1:
                tot_len += State.distances[inxs_taveled[-1],inxs_taveled[-2]]
            print(x)
        print("Number of cities: ",number_of_nodes)
        print("Total length: ",tot_len)
        print("A* search time: ",tot_time," s")
    else:
        print("Solution not found!")