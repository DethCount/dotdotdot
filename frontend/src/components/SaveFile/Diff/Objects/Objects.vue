<template>
    <div class="row">
        <div class="col-12" ref="objectTree" v-if="tree">
            <Tree
                :nodes="tree.entries()"
                :nodeComponent="nodeComponent"
                :scalarComponent="scalarComponent"
                ></Tree>
        </div>
        <div class="col-12" v-else>
            <Spinner></Spinner>
        </div>
    </div>
</template>

<script src="@/components/SaveFile/Diff/Objects/Objects.js"></script>

<style lang="sass" scoped src="@/components/SaveFile/Diff/Objects/Objects.sass"></style>
