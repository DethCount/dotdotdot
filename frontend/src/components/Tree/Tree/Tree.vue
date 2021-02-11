<template>
    <div class="accordion">
        <div v-bind:key="node[0]" v-for="node of nodes">
            <component
                :is="nodeComponent"
                :node="node"
                :nodeComponent="nodeComponent"
                :scalarComponent="scalarComponent"
                ></component>
        </div>
    </div>
</template>

<script src="@/components/Tree/Tree/Tree.js"></script>

<style lang="sass" scoped src="@/components/Tree/Tree/Tree.sass"></style>
