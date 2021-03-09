const state = {
    index: '/'
}

const mutations = {
    SET_INDEX(state, index) {
      state.index = index
    }
}

const actions = {
    setIndex: ({ commit }, index) => {
        commit("SET_INDEX", index);
    }
}

const getters = {
    index: state => {
        return state.index;
    },
}

export default {
    state,
    mutations,
    actions,
    getters
};
